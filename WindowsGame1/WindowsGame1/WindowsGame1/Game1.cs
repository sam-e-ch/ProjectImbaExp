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
        
        Vector2 centerPoint;
        Sprite background;
       
        SpriteFont calibri;
        int fps;
  
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            centerPoint = new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
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

            spaceShip.LoadContent(this.Content, "images/shuttle");
            background.LoadContent(this.Content, "images/stars");
            calibri = Content.Load<SpriteFont>("calibri");

            background.Size = 2.5f;
            spaceShip.SetPosition(centerPoint);  
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
            KeyboardState keyboard = Keyboard.GetState();

            Boolean left = keyboard.IsKeyDown(Keys.Left);
            Boolean right = keyboard.IsKeyDown(Keys.Right);
            Boolean up = keyboard.IsKeyDown(Keys.Up);
            Boolean down = keyboard.IsKeyDown(Keys.Down);
            Boolean speedUp = keyboard.IsKeyDown(Keys.X);
            Boolean speedDown = keyboard.IsKeyDown(Keys.Y);
            Boolean reset = keyboard.IsKeyDown(Keys.Enter);
            Boolean finish = keyboard.IsKeyDown(Keys.Escape);
            Boolean shoot = keyboard.IsKeyDown(Keys.Space);
            Boolean fullScreenKeys = keyboard.IsKeyDown(Keys.F11);

            if (fullScreenKeys) graphics.ToggleFullScreen();

            if (finish) this.Exit();

            float rotateStep = 0.15f;
            
            if (right && !left) this.spaceShip.RotateRight(rotateStep);
            if (left && !right) this.spaceShip.RotateLeft(rotateStep);
  

            if (up && !down)this.spaceShip.ThrustForward(0.1f);
            if (reset) spaceShip.Reset(centerPoint);

            if (shoot) spaceShip.Shoot();
           
            fps = (gameTime.ElapsedGameTime.Milliseconds);

            spaceShip.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            background.Draw(this.spriteBatch);
            spaceShip.Draw(this.spriteBatch);
            
            spriteBatch.DrawString(calibri, ("Lasers: " + spaceShip.LaserCount + "\nFPS: " + fps), new Vector2(10, 10), Color.LightGreen,
       0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
