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

        float[] speed = new float[bodyCount];
        float[] jumpHeight = new float[bodyCount];
        float[] maxSpeed = new float[bodyCount];

        Body[] bodys = new Body[bodyCount];
        PhysicsSprite[] sprites = new PhysicsSprite[bodyCount];
        List<PhysicsSprite> level = new List<PhysicsSprite>();
        Body[,] jumpSensor = new Body[bodyCount, 3];
        Color[] playerColor = new Color[bodyCount];

        bool[] bodyHasContact = new bool[bodyCount];
        private float[] jumpingSpeed = new float[bodyCount];
        private Keys[,] controls;

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

            Vector2[] size = new Vector2[bodyCount];
            Vector2[] convertedSize = new Vector2[bodyCount];
            Vector2[] position = new Vector2[bodyCount];

            int player = 0;

            size[player] = new Vector2(30f, 30f);
            convertedSize[player] = ConvertUnits.ToSimUnits(size[player]);
            position[player] = new Vector2(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(300));

            bodys[player] = BodyFactory.CreateRectangle(world, convertedSize[player].X, convertedSize[player].Y, 13f, position[player]);
            bodys[player].BodyType = BodyType.Dynamic;
            bodys[player].Friction = 1f;
            bodys[player].FixedRotation = true;

            playerColor[player] = Color.Blue;

            player = 1;

            size[player] = new Vector2(30f, 30f);
            convertedSize[player] = ConvertUnits.ToSimUnits(size[player]);
            position[player] = new Vector2(ConvertUnits.ToSimUnits(500), ConvertUnits.ToSimUnits(300));

            bodys[player] = BodyFactory.CreateRectangle(world, convertedSize[player].X, convertedSize[player].Y, 13f, position[player]);
            bodys[player].BodyType = BodyType.Dynamic;
            bodys[player].Friction = 1f;
            bodys[player].FixedRotation = true;

            playerColor[player] = Color.Red;

            controls= new Keys[,]{{Keys.A, Keys.D, Keys.W},{Keys.Left, Keys.Right, Keys.Up}};

            for (int i = 0; i < bodyCount; i++)
            {
                bodyHasContact[i] = false;
                speed[i] = 1f;
                jumpHeight[i] = 4f;
                jumpingSpeed[i] = 0;
                maxSpeed[i] = 3f;
                bodys[i].OnCollision += OnCollision;
                bodys[i].OnSeparation += OnSeperation;
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

        private bool bottomCollision(Vector2 dir)
        {
            if (Math.Abs(dir.X) < Math.Abs(dir.Y))
            {
                if (dir.Y > 0)
                {
                    return true;
                }                
            }

            return false;
        }

        private Vector2 getContactNormal(Contact contact)
        {
            Vector2 normal = new Vector2();
            FixedArray2<Vector2> points = new FixedArray2<Vector2>();
            contact.GetWorldManifold(out normal, out points);

            return normal;
        }

        public bool OnCollision(Fixture fix1, Fixture fix2, Contact contact)
        {
            
            for (int i = 0; i < bodyCount; i++)
            {
                if (fix1.Body == bodys[i] && bottomCollision(getContactNormal(contact)))
                {
                    bodyHasContact[i] = true;
                    break;
                }
            }
            return true;
        }

        public void OnSeperation(Fixture fix1, Fixture fix2)
        {
            ContactEdge tempEdge= fix1.Body.ContactList;
            for (int i = 0; i < bodyCount; i++)
            {
                if (fix1.Body == bodys[i])
                {
                    bodyHasContact[i] = false;

                    while (tempEdge!=null)
                    {
                        if (tempEdge.Contact.IsTouching() && bottomCollision(getContactNormal(tempEdge.Contact)))
                        {
                            bodyHasContact[i] = true;
                            break;
                        }
                        tempEdge = tempEdge.Next;
                    }
                    if (bodyHasContact[i])
                    {
                        break;
                    }                    
                }
            }
        } 
       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            world.Step(0.0166666f);

            input = Keyboard.GetState();
            debugSwitchCooldown--;

            HandlePlayers();

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

        private void HandlePlayers()
        {
            for (int player = 0; player < bodyCount; player++)
            {
                if (input.IsKeyDown(controls[player,0]))
                {
                    if (bodyHasContact[player])
                    {
                        bodys[player].LinearVelocity += new Vector2(-speed[player], 0);
                    }
                    else
                    {
                        bodys[player].LinearVelocity += new Vector2(-Math.Abs(jumpingSpeed[player]), 0);
                    }
                }

                if (input.IsKeyDown(controls[player, 1]))
                {
                    if (bodyHasContact[player])
                    {
                        bodys[player].LinearVelocity += new Vector2(speed[player], 0);
                    }
                    else
                    {
                        bodys[player].LinearVelocity += new Vector2(Math.Abs(jumpingSpeed[player]), 0);
                    }
                }

                if (input.IsKeyDown(controls[player, 2]) && bodyHasContact[player])
                {
                    bodys[player].ApplyLinearImpulse(new Vector2(0f, -jumpHeight[player]));
                    jumpingSpeed[player] = bodys[player].LinearVelocity.X;
                }
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

                spriteBatch.DrawString(calibri, ("Contact P1 " + bodyHasContact[0] + "\nContact P2 " + bodyHasContact[1]), new Vector2(10, 5), Color.Yellow,
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
