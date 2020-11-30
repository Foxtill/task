using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class LogoutRequest : Request
    {
        public string token { get; set; }
    }
}
