using System;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace IO.Components.Converter
{
    public class NewtonJsonConverter : IConverter
    {
        public byte[] GetBytes(object o)
        {
            return Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(o));
        }

        public object GetObject(Type t, byte[] b)
        {
            return JsonConvert.DeserializeObject(
                Encoding.UTF8.GetString(b), t);
        }
    }
}
