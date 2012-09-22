using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FarseerTest.Graphics
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float Rotation { get; set; }
        public string AssetName { get; set; }
        protected float layerDepth = 1f;
        public Color color;

        public Sprite(string textureName, Color _color)
        {
            Rotation = 0f;
            Position = Vector2.Zero;
            AssetName = textureName;
            Texture = null;
            this.color = _color;
        }

        public void LoadContent(ContentManager cm)
        {
            this.Texture = cm.Load<Texture2D>(this.AssetName);
        }

        public static Vector2 texOffset(int width, int height)
        {
            return new Vector2(-width / 2, -height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.Texture != null)
            {
                spriteBatch.Draw(this.Texture, Position, null, this.color, this.Rotation, -texOffset(this.Texture.Width, this.Texture.Height), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
