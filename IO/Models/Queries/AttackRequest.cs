using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class AttackRequest:Request
    {
        public string token { get; set; }
        public float attack_speed = 4f;
    }
}
