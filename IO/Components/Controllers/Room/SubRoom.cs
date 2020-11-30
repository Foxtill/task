using IO.Models.Room;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Components.Controllers.Room
{
    public class SubRoom
    {
        public List<Player> listPlayer = new List<Player>();
        public List<Bullet> bullets = new List<Bullet>();
        public string mode { get; set; }
        public SubRoom()
        {
        }
        public ConcurrentDictionary<string, List<Player>> CreateFirstRoom(ConcurrentDictionary<string, List<Player>> Rooms, List<Player> players, string key)
        {
            Rooms.TryAdd(key, players);
            return Rooms;
        }
        public ConcurrentDictionary<string, List<Player>> CreateNextRoom(ConcurrentDictionary<string, List<Player>> Rooms, List<Player> players, string key)
        {
            Rooms.TryAdd(key, players);
            return Rooms;
        }
    }
}
