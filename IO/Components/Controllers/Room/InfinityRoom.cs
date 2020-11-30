using IO.Models.Room;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IO.Components.Controllers.Room
{
    public class InfinityRoom:SubRoom
    {
        public ConcurrentDictionary<string, List<Player>> infinityRoomsDictionary = new ConcurrentDictionary<string, List<Player>>();
        public InfinityRoom()
        {
            mode = "infinity";
        }
        
    }
}
