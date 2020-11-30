using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace IO.Components.Server
{
    public interface IServer
    {
        void Start(object info = null);

        void Stop(object info = null);

        void Send(object message, IPEndPoint to);

        void HandleRequest(object request, IPEndPoint from);

        IServer AttachLogger(Logger.ILogger logger);
        IServer AttachConverter(Converter.IConverter convert);

        IServer AddController<T>() where T : Controller;
    }
}
