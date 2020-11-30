using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class Response
    {
        public string method { get; set; }

        public Response()
        {

        }

        public Response(string method)
        {
            this.method = method;
        }
    }
}
