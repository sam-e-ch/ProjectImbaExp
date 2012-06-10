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
		public Vector2 Position { get; private set; }
		public float Speed { get; set; }

		private Rectangle view;

		private Vector2 _target;
		public Vector2 Target { get { return _target; } set { _target = value; Update(); } }

		public Camera(Rectangle view)
		{
			this.view = view;
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
			this.Target = obj.Position;
		}

		private void Update()
		{
			this.Position = Target - new Vector2(view.Width / 2, view.Height / 2);
		}
	}
}
