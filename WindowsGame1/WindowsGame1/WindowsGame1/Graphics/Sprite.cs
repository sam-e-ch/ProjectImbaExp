using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace WindowsGame1.Graphics
{
    class Sprite
    {
        protected Vector2 Position = new Vector2(0, 0);
        protected Texture2D spriteTexture;
        public int Speed { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Size { get; set; }
        protected float layerDepth;

        public Sprite()
        {
            Speed = 1;
            Rotation = 0f;           
            layerDepth = 1;
            Origin = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            spriteTexture = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void SetPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public void SetPosition(Vector2 vect)
        {
            Position = vect;
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
            Rotation += amount*Speed/3;
        }

        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }

        public void MoveForward()
        {
            Position.X += (float)(Speed*Math.Sin(Rotation));
            Position.Y -= (float)(Speed * Math.Cos(Rotation));
        }

        public void MoveBackward()
        {
            Position.X -= (float)(Speed * Math.Sin(Rotation));
            Position.Y += (float)(Speed * Math.Cos(Rotation));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, Position,null, Color.White, Rotation, Origin, Size, SpriteEffects.None, layerDepth);
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
