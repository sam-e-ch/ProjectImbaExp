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
using WindowsGame1.Input;

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

		//float size;

		TiledSprite background;

		Camera camera;
		bool track = true;
		bool mute = false;

		SpriteFont calibri;

		Effect moblur;
		InputState input;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);

			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.ApplyChanges();

			this.IsFixedTimeStep = true;

			Content.RootDirectory = "Content";

			background = new TiledSprite("images/stars", 9);
			spaceShip = new SpaceShip();

			camera = new Camera(GraphicsDevice.Viewport.Bounds);		
			input = new InputState();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			spaceShip.Position = Vector2.Zero;			
			background.Position = Vector2.Zero;
			background.Size = 3.0f;

			camera.Speed = 200.0f;
			camera.Inertia = 0.5f;
			camera.Zoom = 1.0f;
			camera.Track(spaceShip);

			SoundEffect.MasterVolume = 0.15f;

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
			background.LoadContent(this.Content);

			moblur = Content.Load<Effect>("MotionBlur");
			calibri = Content.Load<SpriteFont>("calibri");
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
			input.Update();

			if (input.isPressed(Keys.W)) spaceShip.ThrustForward(3000.0f);
			if (input.isPressed(Keys.A)) spaceShip.RotateLeft(12.0f); ;
			if (input.isPressed(Keys.S)) spaceShip.ThrustBackward(3000.0f);
			if (input.isPressed(Keys.D)) spaceShip.RotateRight(12.0f);

			if (input.isPressed(Keys.Space)) spaceShip.Shoot();
			if (input.isPressed(Keys.Enter)) spaceShip.Reset();

			if (input.isToggled(Keys.O)) {
				track = !track;

				if (track) camera.Track(spaceShip);
				else camera.UnTrack();
			}

			if (input.isToggled(Keys.M)) ToggleMute();

			if (input.isToggled(Keys.F11)) graphics.ToggleFullScreen();
			if (input.isToggled(Keys.Escape)) Exit();

			if (!track) camera.Move(input.getMouseOffset());

			//size += (float)gameTime.ElapsedGameTime.TotalSeconds;
			//moblur.Parameters["size"].SetValue(size);
			moblur.Parameters["vel"].SetValue(camera.Velocity / 1000.0f * (float)Math.Pow(2, camera.Velocity.Length() / 400.0f));

			int val;
			if (input.getWheelOffset(out val))
				camera.Zoom += val / 2000.0f;
			if (input.isWheelClicked()) camera.Zoom = 1.0f;

			camera.Update(dt);
			spaceShip.Update(dt);

			//if (!camera.isVisible(spaceShip.BoundingBox))
				//this.Exit();

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

			spriteBatch.Begin(0, BlendState.Opaque, null, null, null, moblur, camera.Transform);
			background.Draw(this.spriteBatch, camera);
			spriteBatch.End();

			spriteBatch.Begin(0, null, null, null, null, null, camera.Transform);
			spaceShip.Draw(this.spriteBatch, camera);
			spriteBatch.End();

			spriteBatch.Begin();
			spriteBatch.DrawString(calibri, String.Format("Lasers: {0}\nDrawTime: {1:0.0} ms\nSpeed: {2:000.0}, ASpeed: {3:000.0}", spaceShip.LaserCount, dt * 1000, spaceShip.Speed, spaceShip.AngularVelocity),
				new Vector2(10, 10), Color.LightGreen, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void ToggleMute()
		{
			if (!mute) Mute();
			else UnMute();

			mute = !mute;
		}

		public void Mute()
		{
			SoundEffect.MasterVolume = 0;
		}

		public void UnMute()
		{
			SoundEffect.MasterVolume = 0.15f;
		}
	}
}
