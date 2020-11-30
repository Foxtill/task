using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IO.Models.Room
{
    public class Bullet
    {
        public string token { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        internal float u { get; set; }
        internal float Angle { get; set; }
        internal void SetSettings(Player player)
        {
            this.token = player.Token;
            this.X = player.x;
            this.Y = player.y;
            this.Angle = player.angle;
        }
        internal void ChangePos()
        {
            X += u * MathF.Cos(Angle);
            Y += u * MathF.Sin(Angle);
        }
    }
}
