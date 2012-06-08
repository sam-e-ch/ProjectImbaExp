using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Graphics
{
    class SpaceShip : Sprite
    {

        public SpaceShip()
            : base()
        {

        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);
            Origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
        }

        public void MoveRight(int amount)
        {
            Position.X += amount;
        }

        public void MoveLeft(int amount)
        {
            this.MoveRight(-amount);
        }

        public void MoveDown(int amount)
        {
            Position.Y += amount;
        }

        public void MoveUp(int amount)
        {
            this.MoveDown(-amount);
        }

        public void MoveRight()
        {
            Position.X += Speed;
        }

        public void MoveLeft()
        {
            Position.X -= Speed;
        }

        public void MoveDown()
        {
            Position.Y += Speed;
        }

        public void MoveUp()
        {
            Position.Y -= Speed;
        }

        public void RotateRight(float amount)
        {
            Rotation += amount * Speed / 3;
        }

        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }

        public void MoveForward()
        {
            Position.X += (float)(Speed * Math.Sin(Rotation));
            Position.Y -= (float)(Speed * Math.Cos(Rotation));
        }

        public void MoveBackward()
        {
            Position.X -= (float)(Speed * Math.Sin(Rotation));
            Position.Y += (float)(Speed * Math.Cos(Rotation));
        }        

        public void IncreaseSpeed()
        {
            if (Speed <= 10)
            {
                this.Speed++;
            }
        }

        public void DecreaseSpeed()
        {
            if (Speed > 0)
            {
                this.Speed--;
            }
        }
    }


}
