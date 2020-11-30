using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

using IO.Models.Queries;

using Newtonsoft.Json.Linq;

namespace IO.Components.Server
{
    public class UdpServer : GameObject, IServer
    {
        private int _port;

        private UdpClient _udp;
        private UdpState _state;

        private Dictionary<string, List<ControllerMethod>> MethodsVoid { get; set; }
        private Dictionary<string, List<ControllerMethod>> MethodsObject { get; set; }

        private Converter.IConverter _converter { get; set; }
        private Logger.ILogger _logger { get; set; }

        public UdpServer()
        {
            MethodsVoid = new Dictionary<string, List<ControllerMethod>>();
            MethodsObject = new Dictionary<string, List<ControllerMethod>>();
        }

        public IServer AttachLogger(Logger.ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IServer AttachConverter(Converter.IConverter converter)
        {
            _converter = converter;
            return this;
        }

        public IServer AddController<T>() where T : Controller
        {
            Type controllerType = typeof(T);
            Controller controller = (Controller) controllerType
                .GetConstructor(new Type[] { typeof(IServer), typeof(Converter.IConverter), typeof(Logger.ILogger) })
                .Invoke(new object[] { this, _converter, _logger });

            foreach (var method in controllerType.GetMethods())
            {
                Utils.Attributes.MethodAttribute attribute =
                    method.GetCustomAttribute<Utils.Attributes.MethodAttribute>(false);
                
                if (attribute == null)
                {
                    continue;
                }

                string command = attribute.Command;

                ParameterInfo[] parameters = method.GetParameters();

                if (parameters.Count() != 2 || 
                    parameters[0].ParameterType != typeof(string) || 
                    parameters[1].ParameterType != typeof(IPEndPoint))
                {
                    continue;
                }

                if (method.ReturnType == typeof(void))
                {
                    ControllerMethod controllerMethod = new ControllerMethod();
                    if (!MethodsVoid.ContainsKey(command))
                    {
                        MethodsVoid.Add(command, new List<ControllerMethod>());
                    }

                    controllerMethod.ReturnType = typeof(void);
                    controllerMethod.FirstParam = parameters[0].ParameterType;

                    controllerMethod.Void = (ControllerMethod.MethodVoid)
                        Delegate.CreateDelegate(typeof(ControllerMethod.MethodVoid), controller, method);

                    MethodsVoid[command].Add(controllerMethod);
                }

                if (method.ReturnType.IsClass)
                {
                    ControllerMethod controllerMethod = new ControllerMethod();
                    if (!MethodsObject.ContainsKey(command))
                    {
                        MethodsObject.Add(command, new List<ControllerMethod>());
                    }

                    controllerMethod.ReturnType = method.ReturnType;
                    controllerMethod.FirstParam = parameters[0].ParameterType;
                    controllerMethod.Object = (ControllerMethod.MethodObject) 
                        Delegate.CreateDelegate(typeof(ControllerMethod.MethodObject), controller, method);

                    MethodsObject[command].Add(controllerMethod);
                }

            }

            return this;
        }

        public void Start(object info)
        {
            try
            {
                _port = (int)info;
            }
            catch (InvalidCastException)
            {
                _logger?.Log("udpserver", "Invalid port", Logger.ILogger.MessageLevel.Error);
                return;
            }

            _udp = new UdpClient(_port);
            _logger?.Log("udpserver", $"Server started on port {_port}", Logger.ILogger.MessageLevel.Common);

            _state = new UdpState(_udp);
            _udp.BeginReceive(RecieveCallBack, _state);
        }

        public void Stop(object info)
        {
            _udp?.Close();
            _logger?.Log("udpserver", $"Server stoped on port {_port}", Logger.ILogger.MessageLevel.Common);
        }

        public void Send(object message, IPEndPoint to)
        {
            byte[] bytes = _converter.GetBytes(message);
            lock (_lock)
            {
                _udp.Send(bytes, bytes.Length, to);
            }
        }

        public void HandleRequest(object request, IPEndPoint from)
        {
            
        }

        private void RecieveCallBack(IAsyncResult r)
        {
            UdpState state = r.AsyncState as UdpState;

            UdpClient u = state.udp;
            IPEndPoint e = state.endPoint;

            try
            {
                byte[] receiveBytes;
                lock (_lock)
                {
                    receiveBytes = u.EndReceive(r, ref e);
                
                }
                
                // Todo: Добавить метод GetCommand у IConverter
                string temp = Encoding.UTF8.GetString(receiveBytes);
                string command = JObject.Parse(temp)
                    .SelectToken("method").ToString();

                new Task(() =>
                {
                    if (MethodsVoid.ContainsKey(command))
                    {
                        foreach (ControllerMethod method in MethodsVoid[command])
                        {
                            method.Void(Encoding.UTF8.GetString(receiveBytes), e); // _converter.GetObject(method.FirstParam, receiveBytes)
                        }
                    }

                    if (MethodsObject.ContainsKey(command))
                    {
                        foreach (ControllerMethod method in MethodsObject[command])
                        {
                            Send(method.Object(Encoding.UTF8.GetString(receiveBytes), e), e);  //method.Object(_converter.GetObject(method.FirstParam, receiveBytes), e), e);
                        }
                    }

                }).Start();
            }
            catch (Exception)
            {
                _logger?.Log("udpserver", $"Server restared", Logger.ILogger.MessageLevel.Exception);

                lock (_lock)
                {
                    u.Close();

                    u = new UdpClient(_port);
                    _state = new UdpState(u);
                }
            }
            finally
            {
                u.BeginReceive(RecieveCallBack, state);
            }
        }

        private class UdpState
        {
            public UdpClient udp;
            public IPEndPoint endPoint;

            public UdpState(UdpClient _udp)
            {
                udp = _udp;
            }
        }

        private class ControllerMethod
        {
            public delegate void MethodVoid(string o, IPEndPoint from);
            public delegate object MethodObject(string o, IPEndPoint from);

            public Type ReturnType;
            public Type FirstParam;
            public MethodVoid Void;
            public MethodObject Object;
        }
    }
}
