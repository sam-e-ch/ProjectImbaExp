using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Threading.Tasks;
using WindowsGame1.Graphics;

namespace WindowsGame1
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpaceShip spaceShip;

		Sprite background;

		Camera camera;
		bool trackSpaceShip = true;

		SpriteFont calibri;
		Vector2 oldMousePos;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.SynchronizeWithVerticalRetrace = false;

			graphics.ApplyChanges();

			camera = new Camera(GraphicsDevice.Viewport.Bounds);
			camera.Speed = 3.0f;

			this.IsFixedTimeStep = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			spaceShip = new Graphics.SpaceShip(this);
			background = new Graphics.Sprite();

			SoundEffect.MasterVolume = 0.15f;
			background.Position = Vector2.Zero;
			spaceShip.Position = Vector2.Zero;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			spaceShip.LoadContent(this.Content);
			background.LoadContent(this.Content, "images/stars");
			calibri = Content.Load<SpriteFont>("calibri");

			background.Size = 5.0f;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			double dt = gameTime.ElapsedGameTime.TotalSeconds;

			KeyboardState keyboard = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
			Vector2 mouseDelta = mousePos - oldMousePos;

			oldMousePos = mousePos;

			if (!trackSpaceShip)
				camera.Move(mouseDelta);
			else
				camera.Track(spaceShip);

			Keys[] keys = keyboard.GetPressedKeys();
			foreach (Keys k in keys)
			{
				switch (k)
				{
					case Keys.W:
						this.spaceShip.ThrustForward(3000.0f); break;
					case Keys.A:
						this.spaceShip.RotateLeft(12.0f); break;
					case Keys.S:
						this.spaceShip.ThrustBackward(3000.0f); break;
					case Keys.D:
						this.spaceShip.RotateRight(12.0f); break;

					case Keys.Space:
						spaceShip.Shoot(); break;
					case Keys.Enter:
						spaceShip.Reset(); break;

					case Keys.O:
						trackSpaceShip = true; break;
					case Keys.P:
						trackSpaceShip = false; break;
					case Keys.M:
						Mute(); break;

					case Keys.F11:
						graphics.ToggleFullScreen(); break;
					case Keys.Escape:
						this.Exit(); break;
				}
			}

			spaceShip.Update(gameTime.ElapsedGameTime.TotalSeconds);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			double dt = gameTime.ElapsedGameTime.TotalSeconds;

			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();

			background.Draw(this.spriteBatch, camera);
			spaceShip.Draw(this.spriteBatch, camera);

			spriteBatch.DrawString(calibri, String.Format("Lasers: {0}\nDrawTime: {1:0.0} ms\nSpeed: {2:000.0}, ASpeed: {3:000.0}", spaceShip.LaserCount, dt * 1000, spaceShip.Speed, spaceShip.AngularVelocity),
				new Vector2(10, 10), Color.LightGreen, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void Mute()
		{
			SoundEffect.MasterVolume = 0;
		}
	}
}
