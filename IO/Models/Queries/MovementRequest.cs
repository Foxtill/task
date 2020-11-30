using IO.Models.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class MovementRequest: Request
    {
        public string Token{ get;set; }
        public float dx { get; set; }
        public float dy { get; set; }
        public float angle { get; set; }

    }
}
