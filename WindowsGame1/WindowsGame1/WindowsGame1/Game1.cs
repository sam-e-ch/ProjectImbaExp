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
        int lastShot = 0;
        List<Graphics.Laser> LaserList = new List<Graphics.Laser>();
        SpriteFont calibri;
        int fps;
        SoundEffect laserSound;

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
            // TODO: Add your initialization logic here
            spaceShip = new Graphics.SpaceShip();
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

            // TODO: use this.Content to load your game content here
            spaceShip.LoadContent(this.Content, "images/shuttle");
            background.LoadContent(this.Content, "images/stars");
            background.Size = 2.5f;
            spaceShip.SetPosition(centerPoint);
            calibri = Content.Load<SpriteFont>("calibri");
            laserSound = Content.Load<SoundEffect>("laser_sound");
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
            Boolean up = keyboard.IsKeyDown(Keys.Up);
            Boolean down = keyboard.IsKeyDown(Keys.Down);
            Boolean speedUp = keyboard.IsKeyDown(Keys.X);
            Boolean speedDown = keyboard.IsKeyDown(Keys.Y);
            Boolean reset = keyboard.IsKeyDown(Keys.Enter);
            Boolean finish = keyboard.IsKeyDown(Keys.Escape);
            Boolean shoot = keyboard.IsKeyDown(Keys.Space);
            Boolean fullScreenKeys = keyboard.IsKeyDown(Keys.F11);
            Boolean volumeUp = keyboard.IsKeyDown(Keys.Add);
            Boolean volumeDown = keyboard.IsKeyDown(Keys.Subtract);

            if (fullScreenKeys)
            {
                graphics.ToggleFullScreen();
            }

            if (volumeUp && !volumeDown && SoundEffect.MasterVolume<0.9f)
            {
                SoundEffect.MasterVolume += 0.01f;
            }

            if (!volumeUp && volumeDown && SoundEffect.MasterVolume>0.01f)
            {
                SoundEffect.MasterVolume -= 0.01f;
            }

            if (finish)
            {
                this.Exit();
            }

            float rotateStep = 0.15f;


            if (right && !left)
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
                if (!this.spaceShip.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.spaceShip.MoveBackward();
                }
            }
            else if (down && !up)
            {
                this.spaceShip.MoveBackward();
                if (!this.spaceShip.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.spaceShip.MoveForward();
                }
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

            if (shoot)// && lastShot>5)
            {
                lastShot = -1;
                Graphics.Laser tempLaser = new Graphics.Laser(spaceShip.Rotation-(float)(Math.PI/20), spaceShip.Position);
                tempLaser.LoadContent(this.Content, "images/laser_blue");
                LaserList.Add(tempLaser);

                tempLaser = new Graphics.Laser(spaceShip.Rotation + (float)(Math.PI / 20), spaceShip.Position);
                tempLaser.LoadContent(this.Content, "images/laser_green");
                LaserList.Add(tempLaser);

                tempLaser = new Graphics.Laser(spaceShip.Rotation, spaceShip.Position);
                tempLaser.LoadContent(this.Content, "images/laser_red");
                LaserList.Add(tempLaser);

                laserSound.Play();
            }

            lastShot++;

            Parallel.ForEach(LaserList, laser => laser.NextStep());

            LaserList.RemoveAll(n => !n.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

            fps += 1000/((gameTime.ElapsedGameTime.Milliseconds)>0?gameTime.ElapsedGameTime.Milliseconds:1);
            fps /= 2;

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
            LaserList.ForEach(n => n.Draw(this.spriteBatch));
            spaceShip.Draw(this.spriteBatch);
            
            spriteBatch.DrawString(calibri, ("Lasers: " + LaserList.Count + "\nFPS: " + fps), new Vector2(10, 5), Color.Cyan,
       0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
