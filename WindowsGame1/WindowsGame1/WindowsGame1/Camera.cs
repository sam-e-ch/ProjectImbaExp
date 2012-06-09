using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WindowsGame1.Graphics;

namespace WindowsGame1
{
    class Camera
    {
        public Vector2 Position { get; private set; }
        public float Speed { get; set; }

        private Rectangle view;
        private Vector2 observePoint;

        public Camera(Rectangle view)
        {
            this.view = view;
            this.observePoint = new Vector2(view.Width / 2, view.Height / 2);
        }

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

        public void Move(Vector2 mouseDelta)
        {
            this.Position += Speed * mouseDelta;
        }

        public void Track(ITrackable obj)
        {
            this.Position = obj.getPosition() - observePoint;
        }
    }
}
