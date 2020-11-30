using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class AttackResponse:Response
    {
        public AttackResponse()
        {
            method = "eventOnAttacked";
        }
    }
}
