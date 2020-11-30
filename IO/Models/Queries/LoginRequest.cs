using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class LoginRequest : Request
    {
        public string login_type { get; set; }

        public string token { get; set; }
    }
}
