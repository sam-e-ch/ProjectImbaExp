using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Sprites
{
    class Laser : Sprite
    {
        public int TTL { get; private set; }
        public Laser(float rotation, Vector2 position)
            : base(position,rotation)
        {
            TTL = 1000;
        }
       

        public void NextStep()
        {
            float x = Position.X + (float)(20 * Math.Sin(Rotation));
            float y = Position.Y - (float)(20 * Math.Cos(Rotation));
            Position = new Vector2(x, y);
            TTL--;
        }

    }
}
