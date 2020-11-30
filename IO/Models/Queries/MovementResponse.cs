using IO.Models.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class MovementResponse:Response
    {
        public List<Player> players = new List<Player>();
        public MovementResponse()
        {
            method = "gameloop";
        }
    }
}
