using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Components.Converter
{
    public interface IConverter
    {
        public byte[] GetBytes(object o);

        public object GetObject(Type t, byte[] b);
    }
}
