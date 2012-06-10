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
		public Vector2 Position { get; set; }
		public Texture2D Texture {get; set;}
		public float Rotation { get; set; }
		public float Size { get; set; }
		protected float layerDepth;

		public static Vector2 texOffset(int width, int height, float sizeMultiplier)
		{
			return new Vector2(-width / 2, -height / 2);
		}

		public Sprite()
		{
			Rotation = 0f;
			layerDepth = 1;
		}

		public void LoadContent(ContentManager theContentManager, string theAssetName)
		{
			this.Texture = theContentManager.Load<Texture2D>(theAssetName);
		}

		public virtual void Draw(SpriteBatch spriteBatch, Camera cam)
		{
			spriteBatch.Draw(this.Texture, Position - cam.Position, null, Color.White, Rotation,
				-texOffset(this.Texture.Width, this.Texture.Height, Size), Size, SpriteEffects.None, layerDepth);
		}
	}
}