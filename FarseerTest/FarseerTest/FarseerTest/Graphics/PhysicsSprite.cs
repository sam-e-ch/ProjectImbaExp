using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerTest.Helper;
using Microsoft.Xna.Framework;

namespace FarseerTest.Graphics
{
    class PhysicsSprite : Sprite
    {
        public Body body;
        public Vector2 Size { get; private set; }
        private float maxSpeed = 5f;

        public PhysicsSprite(Body _body, String textureName, Color _color, Vector2 size)
            : base(textureName, _color)
        {
            body = _body;
            Rotation = body.Rotation;
            Position = ConvertUnits.ToDisplayUnits(body.Position);
            this.Size = size;
        }

        public void Update()
        {
            Rotation = body.Rotation;
            Position = ConvertUnits.ToDisplayUnits(body.Position);
            CheckSpeed();
        }

        private void CheckSpeed()
        {
            if (body.LinearVelocity.X > maxSpeed) body.LinearVelocity = new Vector2(maxSpeed, body.LinearVelocity.Y);
            if (body.LinearVelocity.X < -maxSpeed) body.LinearVelocity = new Vector2(-maxSpeed, body.LinearVelocity.Y);

            if (body.LinearVelocity.Y > maxSpeed) body.LinearVelocity = new Vector2(body.LinearVelocity.X, maxSpeed);
            if (body.LinearVelocity.Y < -maxSpeed) body.LinearVelocity = new Vector2(body.LinearVelocity.X, -maxSpeed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(this.Texture, (new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y)), null, this.color, this.Rotation, -texOffset(this.Texture.Width, this.Texture.Height), SpriteEffects.None, 0f);
        }

    }
}
