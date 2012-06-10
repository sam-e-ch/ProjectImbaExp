using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Graphics
{
    class TiledSprite : Sprite
    {
        public int TileCountX { get; set; }
        public int TileCountY { get; set; }

        public TiledSprite(string textureName, int tileCount) : base(textureName)
        {
            this.TileCountX = tileCount;
            this.TileCountY = tileCount;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera cam)
        {
            for (int y = -TileCountY / 2; y < TileCountX / 2; y++)
            {
                for (int x = -TileCountX / 2; x < TileCountX / 2; x++)
                {
                    Vector2 pos = Position + new Vector2(x * Texture.Width, y * Texture.Height) * Size;

                    spriteBatch.Draw(this.Texture, pos - cam.Position, null, Color.White, Rotation,
                        -texOffset(this.Texture.Width, this.Texture.Height, Size), Size, SpriteEffects.None, layerDepth);
                }           
            }                
        }
    }
}
