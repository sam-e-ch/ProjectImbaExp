using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerTest.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerTest.Helper;
using FarseerPhysics.Dynamics.Contacts;
using System;
using FarseerPhysics.Common;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics.Joints;

namespace FarseerTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState input;
        World world;
        SpriteFont calibri;
        DebugViewXNA debugView;

        private bool debugViewFlag = false;
        private int debugSwitchCooldown = 30;

        Vector2 gravity = new Vector2(0f, 9.81f);
        static int bodyCount = 2;
        float speed = 1f;

        Body[] bodys = new Body[bodyCount];
        PhysicsSprite[] sprites = new PhysicsSprite[bodyCount];
        List<PhysicsSprite> level = new List<PhysicsSprite>();
        Body[,] jumpSensor = new Body[bodyCount, 3];
        Color[] playerColor = new Color[bodyCount];

        bool[] bodyHasContact = new bool[bodyCount];
        int[] countBodyContact = new int[bodyCount];
        private float[] jumpingSpeed = new float[bodyCount];

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
            input = new KeyboardState();
            // TODO: Add your initialization logic here
            world = new World(gravity);
            for (int i = 0; i < bodyCount; i++)
            {
                bodyHasContact[i] = false;
                countBodyContact[i] = 0;
                jumpingSpeed[i] = 0;
            }

            Vector2[] size = new Vector2[bodyCount];
            Vector2[] convertedSize = new Vector2[bodyCount];
            Vector2[] position = new Vector2[bodyCount];

            int player = 0;

            size[player] = new Vector2(30f, 30f);
            convertedSize[player] = ConvertUnits.ToSimUnits(size[player]);
            position[player] = new Vector2(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(300));

            bodys[player] = BodyFactory.CreateRectangle(world, convertedSize[player].X, convertedSize[player].Y, 13f, position[player]);
            bodys[player].BodyType = BodyType.Dynamic;
            bodys[player].Friction = 0.5f;
            bodys[player].FixedRotation = true;

            playerColor[player] = Color.Blue;

            player = 1;

            size[player] = new Vector2(30f, 30f);
            convertedSize[player] = ConvertUnits.ToSimUnits(size[player]);
            position[player] = new Vector2(ConvertUnits.ToSimUnits(500), ConvertUnits.ToSimUnits(300));

            bodys[player] = BodyFactory.CreateRectangle(world, convertedSize[player].X, convertedSize[player].Y, 13f, position[player]);
            bodys[player].BodyType = BodyType.Dynamic;
            bodys[player].Friction = 0.5f;
            bodys[player].FixedRotation = true;

            playerColor[player] = Color.Red;

            Vector2 tempOffset;
            for (int j = 0; j < bodyCount; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    tempOffset = new Vector2((ConvertUnits.ToSimUnits(size[j].X / 3 + 3)) * i - convertedSize[j].X / 2 + ConvertUnits.ToSimUnits(2), ConvertUnits.ToSimUnits(size[j].Y / 2 + 2.5f));

                    jumpSensor[j, i] = BodyFactory.CreateRectangle(world, convertedSize[j].X / 3 - ConvertUnits.ToSimUnits(6), ConvertUnits.ToSimUnits(5), 1f);
                    jumpSensor[j, i].Position = bodys[j].LocalCenter + tempOffset;
                    jumpSensor[j, i].FixedRotation = true;
                    jumpSensor[j, i].BodyType = BodyType.Dynamic;
                    jumpSensor[j, i].IsSensor = true;

                    jumpSensor[j, i].IgnoreCollisionWith(bodys[j]);

                    DistanceJoint tempJoint = JointFactory.CreateDistanceJoint(world, bodys[j], jumpSensor[j, i], bodys[j].LocalCenter + tempOffset, jumpSensor[j, i].LocalCenter);
                    tempJoint.Length = 0;
                }
            }

            for (int i = 0; i < bodyCount; i++)
            {
                sprites[i] = new PhysicsSprite(bodys[i], "Graphics/squares", playerColor[i], size[i]);
            }

            InitializeLevel();

            base.Initialize();
        }

        private void InitializeLevel()
        {
            float width = (this.GraphicsDevice.Viewport.Width);
            float height = (this.GraphicsDevice.Viewport.Height);
            Vector2 size = new Vector2(width, 20);

            Body floor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(size.X), ConvertUnits.ToSimUnits(size.Y), 1f);
            floor.BodyType = BodyType.Static;
            floor.Rotation = 0.0f;

            floor.Position = new Vector2(ConvertUnits.ToSimUnits(size.X / 2), ConvertUnits.ToSimUnits(height - size.Y / 2));
            level.Add(new PhysicsSprite(floor, "Graphics/squares", Color.Green, size));

            size = new Vector2(50, 20);

            floor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(size.X), ConvertUnits.ToSimUnits(size.Y), 1f);
            floor.Position = new Vector2(ConvertUnits.ToSimUnits(200), ConvertUnits.ToSimUnits(height - 50));
            level.Add(new PhysicsSprite(floor, "Graphics/squares", Color.Cyan, size));

            size = ConvertUnits.ToSimUnits(new Vector2(5, GraphicsDevice.Viewport.Height));
            floor = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            floor.Position = new Vector2(0, size.Y / 2);
            level.Add(new PhysicsSprite(floor, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));

            floor = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            floor.Position = new Vector2(ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Width), size.Y / 2);
            level.Add(new PhysicsSprite(floor, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugView = new DebugViewXNA(world);
            debugView.LoadContent(GraphicsDevice, Content);

            calibri = Content.Load<SpriteFont>("fonts/calibri");

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].LoadContent(this.Content);
            }

            foreach (PhysicsSprite p in level)
            {
                p.LoadContent(this.Content);
            }

        }

        private void setBodyHasContact()
        {
            for (int j = 0; j < countBodyContact.Length; j++)
            {
                countBodyContact[j] = 0;
                for (int i = 0; i < 3; i++)
                {
                    foreach (PhysicsSprite p in level)
                    {
                        if (jumpSensor[j, i].ContactList != null)
                            if (jumpSensor[j, i].ContactList.Contact.FixtureA.Body.Equals(p.body) || jumpSensor[j, i].ContactList.Contact.FixtureB.Body.Equals(p.body))
                            {
                                countBodyContact[j]++;
                                break;
                            }
                    }

                    for (int a = 0; a < bodyCount; a++)
                    {
                        if (jumpSensor[j, i].ContactList != null && a != j)
                            if (jumpSensor[j, i].ContactList.Contact.FixtureA.Body.Equals(bodys[a]) || jumpSensor[j, i].ContactList.Contact.FixtureB.Body.Equals(bodys[a]))
                            {
                                countBodyContact[j]++;
                                break;
                            }
                    }
                }
                bodyHasContact[j] = countBodyContact[j] >= 2;
            }
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            world.Step(0.016666666666f);

            input = Keyboard.GetState();

            debugSwitchCooldown--;
            setBodyHasContact();

            HandlePlayer1();
            HandlePlayer2();

            if (input.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (input.IsKeyDown(Keys.F3) && debugSwitchCooldown <= 0)
            {
                debugViewFlag = !debugViewFlag;
                debugSwitchCooldown = 30;
            }

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].Update();
            }

            base.Update(gameTime);
        }

        private void HandlePlayer1()
        {
            int player = 0;
            if (input.IsKeyDown(Keys.Left))
            {
                if (bodyHasContact[player])
                {
                    bodys[player].LinearVelocity += new Vector2(-speed, 0);
                }
                else
                {
                    bodys[player].LinearVelocity += new Vector2(-Math.Abs(jumpingSpeed[player]), 0);
                }
            }

            if (input.IsKeyDown(Keys.Right))
            {
                if (bodyHasContact[player])
                {
                    bodys[player].LinearVelocity += new Vector2(speed, 0);
                }
                else
                {
                    bodys[player].LinearVelocity += new Vector2(Math.Abs(jumpingSpeed[player]), 0);
                }
            }

            if (input.IsKeyDown(Keys.Up) && bodyHasContact[player])
            {
                bodys[player].ApplyLinearImpulse(new Vector2(0f, -1f));
                jumpingSpeed[player] = bodys[player].LinearVelocity.X;
            }
        }

        private void HandlePlayer2()
        {
            int player = 1;
            if (input.IsKeyDown(Keys.A))
            {
                if (bodyHasContact[player])
                {
                    bodys[player].LinearVelocity += new Vector2(-speed, 0);
                }
                else
                {
                    bodys[player].LinearVelocity += new Vector2(-Math.Abs(jumpingSpeed[player]), 0);
                }
            }

            if (input.IsKeyDown(Keys.D))
            {
                if (bodyHasContact[player])
                {
                    bodys[player].LinearVelocity += new Vector2(speed, 0);
                }
                else
                {
                    bodys[player].LinearVelocity += new Vector2(Math.Abs(jumpingSpeed[player]), 0);
                }
            }

            if (input.IsKeyDown(Keys.W) && bodyHasContact[player])
            {
                bodys[player].ApplyLinearImpulse(new Vector2(0f, -1f));
                jumpingSpeed[player] = bodys[player].LinearVelocity.X;
            }
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (debugViewFlag)
            {
                var projection = Matrix.CreateOrthographicOffCenter(
                0f,
                 ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width),
                 ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height), 0f, 0f,
                 1f);
                debugView.RenderDebugData(ref projection);

                spriteBatch.DrawString(calibri, ("Contact Count P1 " + countBodyContact[0] + "\nContact Count P2 " + countBodyContact[1]), new Vector2(10, 5), Color.Yellow,
                0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
            else
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].Draw(spriteBatch);
                }
                foreach (PhysicsSprite p in level)
                {
                    p.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
