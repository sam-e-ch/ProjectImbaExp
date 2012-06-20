using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Input
{
	class InputState
	{
		private MouseState currentMouseState, prevMouseState;
		private KeyboardState currentKeyState, prevKeyState;

		public void Update()
		{
			prevKeyState = currentKeyState;
			prevMouseState = currentMouseState;

			currentKeyState = Keyboard.GetState();
			currentMouseState = Mouse.GetState();
		}

		public bool isPressed(Keys key)
		{
			return currentKeyState.IsKeyDown(key);
		}

		public bool isToggled(Keys key)
		{
			return (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key));
		}

		public Vector2 getMousePosition()
		{
			return new Vector2(currentMouseState.X, currentMouseState.Y);
		}

		public Vector2 getMouseOffset()
		{
			return getMousePosition() - new Vector2(prevMouseState.X, prevMouseState.Y);
		}

		public bool getWheelOffset(out int value)
		{
			value = currentMouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
			return value != 0;
		}

		public bool isWheelClicked()
		{
			return this.currentMouseState.MiddleButton == ButtonState.Pressed;
		}
	}	
}
