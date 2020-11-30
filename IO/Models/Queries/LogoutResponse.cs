using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class LogoutResponse : Response
    {
        public string status { get; set; }

        public LogoutResponse()
        {
            method = "logout";
        }
    }
}
