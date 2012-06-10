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
using SpaceShooter.Sprites;
using System.Threading.Tasks;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboard;

        MovingSprite player1;
        MovingSprite player2;

        Sprite background ;

        List<Laser> laserList1 = new List<Laser>();
        List<Laser> laserList2 = new List<Laser>();

        int lastShot1 = 0;
        int lastShot2 = 0;

        int hit1 = 0;
        int hit2 = 0;

        const int shootInterval = 5;
        const float rotateStep = 0.05f;

        SpriteFont calibri;
        int fps;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1200;
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
            Vector2 pos1 = new Vector2(50, this.GraphicsDevice.Viewport.Height - 50);
            Vector2 pos2 = new Vector2(this.GraphicsDevice.Viewport.Width - 50, 50);

            player1 = new MovingSprite(pos1, 0f);
            player2 = new MovingSprite(pos2, (float)Math.PI);

            background = new Sprite(new Vector2(0, 0), 0f);

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

            player1.LoadContent(this.Content, "images/shuttle_red");
            player2.LoadContent(this.Content, "images/shuttle_green");

            background.LoadContent(this.Content, "images/stars");
            background.Scale = 4.5f;

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
            keyboard = Keyboard.GetState();
            Boolean left1 = keyboard.IsKeyDown(Keys.Left);
            Boolean right1 = keyboard.IsKeyDown(Keys.Right);
            Boolean up1 = keyboard.IsKeyDown(Keys.Up);
            Boolean down1 = keyboard.IsKeyDown(Keys.Down);
            Boolean shoot1 = keyboard.IsKeyDown(Keys.RightControl);

            Boolean left2 = keyboard.IsKeyDown(Keys.A);
            Boolean right2 = keyboard.IsKeyDown(Keys.D);
            Boolean up2 = keyboard.IsKeyDown(Keys.W);
            Boolean down2 = keyboard.IsKeyDown(Keys.S);
            Boolean shoot2 = keyboard.IsKeyDown(Keys.LeftShift);

            Boolean fullScreenKeys = keyboard.IsKeyDown(Keys.F11);
            Boolean finish = keyboard.IsKeyDown(Keys.Escape);

            if (fullScreenKeys)
            {
                graphics.ToggleFullScreen();
            }           

            if (finish)
            {
                this.Exit();
            }

            if (left1 && !right1)
            {
                player1.RotateLeft(rotateStep);
            }
            else if (!left1 && right1)
            {
                player1.RotateRight(rotateStep);
            }

            if (up1 && !down1)
            {
                player1.MoveForward();
                if (!this.player1.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.player1.MoveBackward();
                }
            }
            else if (!up1 && down1)
            {
                player1.MoveBackward();
                if (!this.player1.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.player1.MoveForward();
                }
            }

            if (shoot1 && lastShot1 > shootInterval)
            {
                lastShot1 = -1;
                Laser tempLaser = new Laser(player1.Rotation,player1.Position);
                tempLaser.LoadContent(this.Content, "images/laser_red");
                laserList1.Add(tempLaser);
            }


            if (left2 && !right2)
            {
                player2.RotateLeft(rotateStep);
            }
            else if (!left2 && right2)
            {
                player2.RotateRight(rotateStep);
            }

            if (up2 && !down2)
            {
                player2.MoveForward();
                if (!this.player2.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.player2.MoveBackward();
                }
            }
            else if (!up2 && down2)
            {
                player2.MoveBackward();
                if (!this.player2.InField(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
                {
                    this.player2.MoveForward();
                }
            }

            if (shoot2 && lastShot2 > shootInterval)
            {
                lastShot2 = -1;
                Laser tempLaser = new Laser(player2.Rotation, player2.Position);
                tempLaser.LoadContent(this.Content, "images/laser_green");
                laserList2.Add(tempLaser);
            }

            lastShot1++;
            lastShot2++;

            Parallel.ForEach(laserList1, laser => laser.NextStep());
            Parallel.ForEach(laserList2, laser => laser.NextStep());

            laserList1.RemoveAll(n => n.TTL<0);
            laserList2.RemoveAll(n => n.TTL < 0);

            if (player1.IntersectPixels(player2))
            {
                hit1++;
                hit2++;
            }

            List<Laser> tempList = new List<Laser>();

            foreach(Laser laser in laserList1)
            {
                if(laser.IntersectPixels(player2))
                {
                    hit2++;
                    tempList.Add(laser);
                }
            }

            laserList1.RemoveAll(n => tempList.Contains(n));

            tempList = new List<Laser>();

            foreach(Laser laser in laserList2)
            {
                if(laser.IntersectPixels(player1))
                {
                    hit1++;
                    tempList.Add(laser);
                }
            }

            laserList2.RemoveAll(n => tempList.Contains(n));

            fps = 1000 / ((gameTime.ElapsedGameTime.Milliseconds) > 0 ? gameTime.ElapsedGameTime.Milliseconds : 1);            

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

            laserList1.ForEach(n => n.Draw(this.spriteBatch));
            laserList2.ForEach(n => n.Draw(this.spriteBatch));

            player1.Draw(this.spriteBatch);
            player2.Draw(this.spriteBatch);

            spriteBatch.DrawString(calibri, ("FPS: " + fps), new Vector2(10, 5), Color.Yellow,
            0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(calibri, ("Player 1 hits: " + hit1), new Vector2(10, GraphicsDevice.Viewport.Height-20), Color.Red,
            0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(calibri, ("Player 2 hits: " + hit2), new Vector2(GraphicsDevice.Viewport.Width-130, GraphicsDevice.Viewport.Height - 20), Color.Lime,
            0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
