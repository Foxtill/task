using System;
using System.Collections.Generic;
using System.Text;
using IO.Components.Converter;
using IO.Components.Logger;

namespace IO.Components.Server
{
    public class Controller
    {
        protected IServer Server { get; set; }
        protected IConverter Converter { get; set; }
        protected ILogger Logger { get; set; }
     
        public Controller(IServer server, IConverter converter, ILogger logger)
        {
            Server = server;
            Converter = converter;
            Logger = logger;
        }
    }
}
