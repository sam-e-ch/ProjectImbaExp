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
using SideScrollerTest.code.Sprites;

namespace code.SideScrollerTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite[] border = new Sprite[4];
        MovingSprite shuttle;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            border[0] = new Sprite(new Vector2(0, 258), 0f);
            border[1] = new Sprite(new Vector2(0, 0), -(float)Math.PI/2);
            border[2] = new Sprite(new Vector2(258, 0), -(float)Math.PI/2);
            border[3] = new Sprite(new Vector2(0, 0), 0f);

            shuttle = new MovingSprite(new Vector2(50, 100), 0f);           
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
            for(int i = 0; i< border.Length; i++)
            {
                border[i].LoadContent(this.Content, "images/ground");
            }
            shuttle.LoadContent(this.Content, "images/shuttle");
            shuttle.Speed = new Vector2(0, -1);
            
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

            float rotateStep = 0.01f;

            if (right && !left)
            {
                this.shuttle.RotateRight(rotateStep);
            }
            else if (left && !right)
            {
                this.shuttle.RotateLeft(rotateStep);
            }

            if (up && !down)
            {
                shuttle.IncreaseSpeedForward(0.1f);
            }
            else if (down && !up)
            {
                shuttle.DecreaseSpeedForward(0.1f);
            }

            bool intersect = false;

            for(int i = 0; i<border.Length;i++)
            {
                if(shuttle.IntersectPixels(border[i]))
                {
                    intersect = true;
                    break;
                }
            }

            if (!intersect)
            {                
                shuttle.Move();
            }
            else
            {
               shuttle.SetPosition(50, 50);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            shuttle.Draw(spriteBatch);
            for (int i = 0; i < border.Length; i++)
            {
                border[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
