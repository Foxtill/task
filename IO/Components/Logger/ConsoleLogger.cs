using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Components.Logger
{
    public class ConsoleLogger : ILogger
    {
        private static Dictionary<ILogger.MessageLevel, ConsoleColor> colors
            = new Dictionary<ILogger.MessageLevel, ConsoleColor>() {
                { ILogger.MessageLevel.Common, ConsoleColor.Green },
                { ILogger.MessageLevel.Error, ConsoleColor.Red },
                { ILogger.MessageLevel.Exception, ConsoleColor.Yellow },
                { ILogger.MessageLevel.Debug, ConsoleColor.White }};

        public void Log(string tag, string message, ILogger.MessageLevel level)
        {
            Console.ForegroundColor = colors[level];
            Console.WriteLine($"{tag} : {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
