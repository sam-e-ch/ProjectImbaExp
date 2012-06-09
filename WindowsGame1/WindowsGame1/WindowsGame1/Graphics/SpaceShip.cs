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
        private float MAX_VELOCITY = 200.0f;
        private float MAX_ANGULAR_VELOCITY = 5.0f;

        private Vector2 _velocity;
        public Vector2 Velocity { 
            get 
            { 
                return _velocity;
            } 

            private set 
            {
                if (value.Length() >= MAX_VELOCITY)
                    _velocity = _velocity / _velocity.Length() * MAX_VELOCITY;
                else
                    _velocity = value; 
            } 
        }

        public float Speed { get { return Velocity.Length(); } }

        public Vector2 Acceleration { get; set; }

        private float _angularVelocity;
        public float AngularVelocity {
            get 
            {
                return _angularVelocity;
            }

            private set
            {
                if (value >= MAX_ANGULAR_VELOCITY)
                    _angularVelocity = MAX_ANGULAR_VELOCITY;
                else if (value <= -MAX_ANGULAR_VELOCITY)
                    _angularVelocity = -MAX_ANGULAR_VELOCITY;
                else
                    _angularVelocity = value; 
            }
        }

        public float AngularAcceleration { get; set; }

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

        public void RotateRight(float thrust)
        {
            this.AngularAcceleration = thrust;
        }

        public void RotateLeft(float thrust)
        {
            RotateRight(-thrust);
        }

        public void ThrustForward(float thrust)
        {
            this.Acceleration = thrust * (new Vector2((float)Math.Sin(this.Rotation), -(float)Math.Cos(this.Rotation)));
        }

        public void ThrustBackward(float thrust)
        {
            ThrustForward(-thrust);
        }

        public void Update(double dt)
        {
            this.Velocity += this.Acceleration * (float)dt;
            this.Position += this.Velocity * (float)dt;

            this.Rotation += this.AngularVelocity * (float)dt;
            this.AngularVelocity += this.AngularAcceleration * (float)dt;
            
            this.laser.Heading = this.Rotation;
            this.Acceleration = Vector2.Zero;
            this.AngularAcceleration = 0.0f;

            this.laser.Update(dt);
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
            this.AngularVelocity = 0.0f;
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