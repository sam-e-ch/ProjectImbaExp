using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Camera
    {
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        

        public void MoveRight()
        {
            this.Position += new Vector2(Speed, 0);
        }

        public void MoveLeft()
        {
            this.Position += new Vector2(-Speed, 0);
        }

        public void MoveUp()
        {
            this.Position += new Vector2(0, -Speed);
        }

        public void MoveDown()
        {
            this.Position += new Vector2(0, Speed);
        }

        internal void Move(Vector2 mouseDelta, double dt)
        {
            this.Position += Speed * mouseDelta * (float)dt;
        }
    }
}
