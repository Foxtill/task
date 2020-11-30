using IO.Models.Queries;
using IO.Models.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace IO.MovementsData
{
    class WorldUpdate
    {
        public List<Player> Players { get; set; }
        public List<UnitUpdate> Units { get; set; }
    }
    public class UnitUpdate
    {
        public string Token { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float dx { get; set; }
        public float dy { get; set; }
        public float angle { get; set; }
        public float Hp { get; set; }
    }
    public class UpdatePosition
    {
        public UpdatePosition(Player player, MovementRequest movement)
        {
            player.x += movement.dx;
            player.y += movement.dy;
            player.angle = movement.angle;          
        }
    }
}
