using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        public string Command;

        public MethodAttribute(string command)
        {
            Command = command;
        }
    }
}
