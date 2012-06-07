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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Graphics.SpaceShip spaceShip;
        KeyboardState keyboard;
        Vector2 centerPoint;
        Graphics.Sprite background;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1200;
            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spaceShip = new Graphics.SpaceShip();
            background = new Graphics.Sprite();
            centerPoint = new Vector2(this.GraphicsDevice.Viewport.Width/2, this.GraphicsDevice.Viewport.Height/2);
            
            

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
           
            // TODO: use this.Content to load your game content here
            spaceShip.LoadContent(this.Content, "images/shuttle");
            spaceShip.Size = 0.3f;
            background.LoadContent(this.Content, "images/stars");
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
            // TODO: Add your update logic here
            keyboard = Keyboard.GetState();
            Boolean left = keyboard.IsKeyDown(Keys.Left);
            Boolean right = keyboard.IsKeyDown(Keys.Right);
            Boolean up =keyboard.IsKeyDown(Keys.Up);
            Boolean down =keyboard.IsKeyDown(Keys.Down);
            Boolean speedUp = keyboard.IsKeyDown(Keys.X);
            Boolean speedDown = keyboard.IsKeyDown(Keys.Y);
            Boolean reset = keyboard.IsKeyDown(Keys.Enter);
            Boolean finish = keyboard.IsKeyDown(Keys.Escape);

            if (finish)
            {
                this.Exit();
            }

            float rotateStep = 0.1f;


            if (right && !left )
            {
                this.spaceShip.RotateRight(rotateStep);
            }
            else if (left && !right)
            {
                this.spaceShip.RotateLeft(rotateStep);
            }

            if (up && !down)
            {
                this.spaceShip.MoveForward();
            }
            else if (down && !up)
            {
                this.spaceShip.MoveBackward();
            }

            if (speedUp && !speedDown)
            {
                this.spaceShip.IncreaseSpeed();
            }
            else if (speedDown && !speedUp)
            {
                this.spaceShip.DecreaseSpeed();
            }

            if (reset)
            {
                this.spaceShip.SetPosition(centerPoint);
                this.spaceShip.Rotation = 0;
                this.spaceShip.Speed = 1;
            }
            
            
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
