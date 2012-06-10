using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
	public class PhysicsObject
	{
		public float Mass
		{
			get;
			protected set;
		}

		public Vector2 Position
		{
			get;
			set;
		}

		public virtual Vector2 Velocity
		{
			get;
			set;
		}

		public Vector2 Acceleration
		{
			get;
			protected set;
		}

		public float Angle
		{
			get;
			protected set;
		}

		public virtual float AngularVelocity
		{
			get;
			protected set;
		}

		public float AngularAcceleration
		{
			get;
			protected set;
		}

		public void ApplyForce(Vector2 force)
		{
			this.Acceleration = force / this.Mass;
		}

		public virtual void Update(float dt)
		{
			this.Velocity += this.Acceleration * dt;
			this.Position += this.Velocity * dt;

			this.AngularVelocity += this.AngularAcceleration * dt;
			this.Angle += this.AngularVelocity * dt;			
		}	
	}
}
