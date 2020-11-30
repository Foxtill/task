using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Components.Logger
{
    public interface ILogger
    {
        void Log(string tag, string message, MessageLevel level);

        public enum MessageLevel
        {
            Common,
            Exception,
            Error,
            Debug
        }
    }
}
