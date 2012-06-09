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
        public float Size { get; set; }
        protected float layerDepth;

        public static Vector2 texOffset(int width, int height)
        {
            return new Vector2(-width / 2, -height / 2);
        }

        public Sprite()
        {
            Rotation = 0f;
            layerDepth = 1;
        }

        public virtual void LoadContent(ContentManager theContentManager, string theAssetName)
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

        public virtual void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            spriteBatch.Draw(spriteTexture, Position - cam.Position, null, Color.White, Rotation,
                -texOffset(this.spriteTexture.Width, this.spriteTexture.Height), Size, SpriteEffects.None, layerDepth);
        }
    }
}