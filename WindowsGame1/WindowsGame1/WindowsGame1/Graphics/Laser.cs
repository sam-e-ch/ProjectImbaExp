using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Graphics
{
    class Laser : Sprite
    {
        public Laser(float direction, Vector2 position) : base() 
        {
            Rotation = direction;
            Position = position;
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);
            Origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
            this.Size = 1.0f;
        }

        public void NextStep()
        {
            Position.X +=20*(float)(Math.Sin(Rotation));
            Position.Y -= 20*(float)(Math.Cos(Rotation));
        }
        
    }
}
