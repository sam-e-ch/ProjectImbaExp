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
        public Vector2 Acceleration { get; set; }

        public Game game { get; set; }

        public int LaserCount { get { return this.laser.getShotCount(); } }

        private Laser laser;

        public SpaceShip(Game game)
            : base()
        {
            Size = 1f;
            this.laser = new Laser(this);
            this.game = game;
        }

        public override void LoadContent(ContentManager cm, string theAssetName)
        {
            base.LoadContent(cm, theAssetName);
            laser.LoadContent(cm);
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
            this.Acceleration = thrust * (new Vector2((float)Math.Sin(this.Rotation), -(float)Math.Cos(this.Rotation)));
        }

        public void Update()
        {
            this.Position += this.Velocity;
            this.Velocity += this.Acceleration;
            this.Acceleration = Vector2.Zero;

            this.laser.Heading = this.Rotation;
            this.laser.Update();
        }

        public override void Draw(SpriteBatch sp)
        {
            laser.Draw(sp);
            sp.Draw(spriteTexture, Position, null, Color.White, (float)this.Rotation, Origin, Size, SpriteEffects.None, layerDepth);           
        }

        /// <summary>
        /// Resets the spaceship to the origin of space
        /// </summary>
        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
            this.Rotation = 0.0f;
        }

        /// <summary>
        /// Resets the spaceship to the specified point in space
        /// </summary>
        /// <param name="point">The point the spaceship will be reset to</param>
        public void Reset(Vector2 point)
        {
            this.Reset();
            this.Position = point;
        }

        public void Shoot()
        {
            laser.Shoot();
        }
    }
}