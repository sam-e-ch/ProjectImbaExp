using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Graphics
{
    class SpaceShip : Sprite
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public double Heading { get; set; }

        public SpaceShip()
            : base()
        {
            Size = 1f;
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);
            Origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
        }

        public void RotateRight(float amount)
        {
            Rotation += amount;
        }

        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }

        public void ThrustForward(float thrust)
        {
            this.Acceleration = thrust * (new Vector2((float)Math.Cos(this.Heading), (float)Math.Sin(this.Heading)));
        }

        public void Update()
        {
            this.Position += this.Velocity;
            this.Velocity += this.Acceleration;
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(spriteTexture, Position, null, Color.White, (float)this.Heading, Origin, Size, SpriteEffects.None, layerDepth);
        }
    }
}