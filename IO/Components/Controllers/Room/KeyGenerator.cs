using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Components.Controllers.Room
{
    class KeyGenerator
    {
        public string Generate()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < new Random().Next(8, 20); i++)
            {
                builder.Append((char)new Random().Next('A', 'Z' + 1));
            }
            return builder.ToString();
        }
    }
}
