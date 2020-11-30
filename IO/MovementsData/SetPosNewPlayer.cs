using IO.Models.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace IO.MovementsData
{
    class SetPosNewPlayer
    {
        public Player RandomPosition(Player player)
        {
            player.x = new Random().Next(-50, 50);
            player.y = new Random().Next(-50, 50);
            player.angle = new Random().Next(0, 360);
            return player;
        }
    }
}
