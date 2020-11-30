using IO.Models.Room;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class RoomResponse:Response
    {
        public List<Player> players = new List<Player>();
        public string mode { get; set; }
        public string keyRoom { get; set; }
        public RoomResponse()
        {
            method = "connect_room";
        }
    }
}
