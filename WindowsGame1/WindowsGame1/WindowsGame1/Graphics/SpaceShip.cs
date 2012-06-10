using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Graphics
{
	class SpaceShip : PhysicsObject, IDrawable, ITrackable
	{
		private float MAX_VELOCITY = 750.0f;
		private float MAX_ANGULAR_VELOCITY = 7.0f;

		private Sprite sprite;

		private Vector2 _velocity;
		public override Vector2 Velocity
		{
			get
			{
				return _velocity;
			}

			set
			{
				if (value.Length() >= MAX_VELOCITY)
					_velocity = _velocity / _velocity.Length() * MAX_VELOCITY;
				else
					_velocity = value;
			}
		}

		public float Speed { get { return Velocity.Length(); } }

		private float _angularVelocity;
		public override float AngularVelocity
		{
			get
			{
				return _angularVelocity;
			}

			protected set
			{
				if (value >= MAX_ANGULAR_VELOCITY)
					_angularVelocity = MAX_ANGULAR_VELOCITY;
				else if (value <= -MAX_ANGULAR_VELOCITY)
					_angularVelocity = -MAX_ANGULAR_VELOCITY;
				else
					_angularVelocity = value;
			}
		}

		private Game game;
		public int LaserCount { get { return this.laser.getShotCount(); } }

		public Rectangle BoundingBox { get; private set; }

		private Laser laser;

		public SpaceShip(Game game)	: base()
		{
			this.laser = new Laser(this);
			this.laser.FireRate = 15;
			this.game = game;
			this.Mass = 8.0f;
			this.sprite = new Sprite("images/shuttle");
			this.sprite.Size = 1.0f;
		}

		public void LoadContent(ContentManager cm)
		{
			this.sprite.LoadContent(cm);
			laser.LoadContent(cm);
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
			ApplyForce(thrust * new Vector2((float)Math.Sin(this.Angle), -(float)Math.Cos(this.Angle)));
		}

		public void ThrustBackward(float thrust)
		{
			ThrustForward(-thrust);
		}

		public void Update(double dt)
		{
			base.Update((float)dt);

			this.sprite.Position = this.Position;
			this.sprite.Rotation = this.Angle;

			this.laser.Heading = this.Angle;
			this.Acceleration = Vector2.Zero;
			this.AngularAcceleration = 0.0f;

			Vector2 texOffset = Sprite.texOffset(this.sprite.Texture.Width, this.sprite.Texture.Height, this.sprite.Size);

			this.BoundingBox = new Rectangle((int)(this.Position.X + texOffset.X), (int)(this.Position.Y + texOffset.Y), (int)(this.sprite.Texture.Width * this.sprite.Size),
				(int)(this.sprite.Texture.Height * (int)this.sprite.Size));

			this.laser.Update(dt);
		}

		public void Draw(SpriteBatch sp, Camera cam)
		{
			laser.Draw(sp, cam);
			sprite.Draw(sp, cam);
		}

		/// <summary>
		/// Resets the spaceship to the origin of space
		/// </summary>
		public void Reset()
		{
			this.Position = Vector2.Zero;
			this.Velocity = Vector2.Zero;
			this.AngularVelocity = 0.0f;
			this.Angle = 0.0f;
		}

		public void Reset(Vector2 resetPos)
		{
			Reset();
			this.Position = resetPos;
		}

		public void Shoot()
		{
			laser.Shoot();
		}
	}
}