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
        private Vector2 Position = new Vector2(0, 0);
        private Texture2D spriteTexture;

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {

            spriteTexture = theContentManager.Load<Texture2D>(theAssetName);

        }

        public void setPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public void moveRight(int amount)
        {
            Position.X += amount;
        }

        public void moveLeft(int amount)
        {
            this.moveRight(-amount);
        }

        public void moveDown(int amount)
        {
            Position.Y += amount;
        }

        public void moveUp(int amount)
        {
            this.moveRight(-amount);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, Position, Color.White);
        }
    }
}
