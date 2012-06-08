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
        }

        public void NextStep()
        {
            Position.X +=(float)(Math.Sin(Rotation));
            Position.Y += (float)(Math.Cos(Rotation));
        }

        public Boolean InField(int x, int y)
        {
            return (Position.X >= 0 && Position.X < x && Position.Y >= 0 && Position.Y < y);
        }
    }
}
