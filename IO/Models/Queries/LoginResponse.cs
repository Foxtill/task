using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class LoginResponse : Response
    {
        public string status { get; set; }

        public LoginResponse()
        {
            method = "login";
        }
    }
}
