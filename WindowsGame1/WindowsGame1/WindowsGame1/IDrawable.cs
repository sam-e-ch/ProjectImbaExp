using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
	public interface IDrawable
	{
		void Draw(SpriteBatch sp, Camera cam);

		void LoadContent(ContentManager cm);
	}
}
