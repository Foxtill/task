using System;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Room
{
    public class Player : Components.GameObject
    {
        public PlayerAuthLevel AuthLevel { get; private set; }

        public string Token { get; set; }
        public string keyRoom { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float dx { get; set; }
        public float dy { get; set; }
        public float angle { get; set; }
        public string mode { get; set; }
        public void GetFromDB()
        {

        }

        public void SaveToDB()
        {

        }
    }

    public enum PlayerAuthLevel
    {
        Auth,
        Guest,
        Admin
    }
}
