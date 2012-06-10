using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WindowsGame1.Graphics;

namespace WindowsGame1
{
	public class Camera
	{
		public float Speed { get; set; }
		public Vector2 Position { get; private set; }

		public Rectangle View { get; private set; }

		private Vector2 _target;
		public Vector2 Target { get { return _target; } set { _target = value; Update(); } }
		private Vector2 oldTarget;

		public Camera(Rectangle view)
		{
			this.View = view;
			this.Target = Vector2.Zero;
		}

		public void MoveRight()
		{
			this.Target += new Vector2(Speed, 0);
		}

		public void MoveLeft()
		{
			this.Target += new Vector2(-Speed, 0);
		}

		public void MoveUp()
		{
			this.Target += new Vector2(0, -Speed);
		}

		public void MoveDown()
		{
			this.Target += new Vector2(0, Speed);
		}

		public void Move(Vector2 offset)
		{
			this.Target += Speed * offset;
		}

		public void Track(ITrackable obj)
		{
			Vector2 targetOffset = obj.Position - oldTarget;
			this.Target += targetOffset * 0.04f;
		}

		private void Update()
		{
			this.Position = this.Target - new Vector2(this.View.Width / 2, this.View.Height / 2);
			this.View = new Rectangle((int)Position.X, (int)Position.Y, this.View.Width, this.View.Height);
			this.oldTarget = this.Target;
		}

		public bool isVisible(Rectangle r)
		{
			return r.Intersects(this.View);
		}
	}
}
