using System;
using IO.Components.Server;
using IO.Components.Logger;
using IO.Components.Converter;

namespace IO
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new UdpServer()
                .AttachLogger(new ConsoleLogger())
                .AttachConverter(new NewtonJsonConverter())
                .AddController<Components.Controllers.Room.Room>();

            server.Start(9800);
            
            Console.ReadLine();
        }
    }
}
