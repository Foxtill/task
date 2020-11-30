using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Queries
{
    public class PlayerListInRoomResponse:Response
    {
        public ConcurrentDictionary<string, int> tableScore = new ConcurrentDictionary<string, int>();
        public PlayerListInRoomResponse()
        {
            method = "getPlayerList";
        }
    }
}
