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
        public Vector2 Position;
        protected Texture2D spriteTexture;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Size { get; set; }
        protected float layerDepth;

        public Sprite()
        {
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, Position, null, Color.White, Rotation, Origin, Size, SpriteEffects.None, layerDepth);
        }
    }

}