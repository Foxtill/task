using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class InfoResponse : Response
    {
        public string server_location { get; set; }
        public int online { get; set; }

        public InfoResponse()
        {
            method = "info";
        }
    }
}
