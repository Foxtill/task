using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IO.Components
{
    public class GameObject
    {
        public long _id { get; private set; }
        public object _lock { get; private set; }

        public GameObject()
        {
            _id = Interlocked.Increment(ref IdSeq);
            _lock = new object();
        }

        private static long IdSeq = 0;
    }
}