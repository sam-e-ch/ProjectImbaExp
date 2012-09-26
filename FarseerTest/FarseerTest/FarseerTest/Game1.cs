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
        int fps;

        Vector2 gravity = new Vector2(0f, 9.81f);
        static int bodyCount = 2;

        float[] speed = new float[bodyCount];
        float[] jumpHeight = new float[bodyCount];
        float[] maxSpeed = new float[bodyCount];

        Body[] bodys = new Body[bodyCount];
        PhysicsSprite[] sprites = new PhysicsSprite[bodyCount];
        List<PhysicsSprite> level = new List<PhysicsSprite>();
        Body[,] jumpSensor = new Body[bodyCount, 3];
        Body b;

        bool[] bodyHasContact = new bool[bodyCount];
        private float[] jumpingSpeed = new float[bodyCount];
        private Keys[,] controls = new Keys[bodyCount,3];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            input = new KeyboardState();
            world = new World(gravity);

            InitializeCharacters();
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

            size = ConvertUnits.ToSimUnits(new Vector2(50, 5));
            Body temp =BodyFactory.CreateRectangle(world, size.X, size.Y,10f);
            Vector2 pos =ConvertUnits.ToSimUnits(new Vector2(300,430));
            temp.Position = pos;
            temp.BodyType = BodyType.Dynamic;

            FixedRevoluteJoint tempJoin = JointFactory.CreateFixedRevoluteJoint(world, temp, temp.LocalCenter, temp.WorldCenter);
            tempJoin.MotorSpeed = 10f;
            tempJoin.MaxMotorTorque = 1000f;
            tempJoin.MotorEnabled = true;

            level.Add(new PhysicsSprite(temp, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));

            size = ConvertUnits.ToSimUnits(new Vector2(5, 50));
            Body temp2 = BodyFactory.CreateRectangle(world, size.X, size.Y, 10f);
            pos = ConvertUnits.ToSimUnits(new Vector2(300, 430));
            temp2.Position = pos;
            temp2.BodyType = BodyType.Dynamic;
            temp2.IgnoreGravity = true;

            AngleJoint aj = JointFactory.CreateAngleJoint(world, temp, temp2);
            aj.TargetAngle = 0;
            FixedDistanceJoint dj = JointFactory.CreateFixedDistanceJoint(world, temp2, temp2.LocalCenter, temp2.WorldCenter);
            dj.Length = 0;

            level.Add(new PhysicsSprite(temp, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));

            size = ConvertUnits.ToSimUnits(new Vector2(50, 5));
            Body temp3 = BodyFactory.CreateRectangle(world, size.X, size.Y, 10f);
            pos = ConvertUnits.ToSimUnits(new Vector2(340, 430));
            temp3.Position = pos;
            temp3.BodyType = BodyType.Dynamic;

            FixedRevoluteJoint tempJoin3 = JointFactory.CreateFixedRevoluteJoint(world, temp3, temp3.LocalCenter, temp3.WorldCenter);
            tempJoin3.MotorSpeed = -10f;
            tempJoin3.MaxMotorTorque = 1000f;
            tempJoin3.MotorEnabled = true;

            level.Add(new PhysicsSprite(temp3, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));

            size = ConvertUnits.ToSimUnits(new Vector2(5, 50));
            Body temp4 = BodyFactory.CreateRectangle(world, size.X, size.Y, 10f);
            temp4.Position = pos;
            temp4.BodyType = BodyType.Dynamic;
            temp4.IgnoreGravity = true;

            AngleJoint aj2 = JointFactory.CreateAngleJoint(world, temp3, temp4);
            aj2.TargetAngle = 0;
            FixedDistanceJoint dj2 = JointFactory.CreateFixedDistanceJoint(world, temp4, temp4.LocalCenter, temp4.WorldCenter);
            dj2.Length = 0;

            level.Add(new PhysicsSprite(temp4, "Graphics/squares", Color.Green, ConvertUnits.ToDisplayUnits(size)));
        }

        private void InitializeCharacters()
        {
            Vector2 size = new Vector2();
            Vector2 position = new Vector2();
            Keys[] keys;

            size = new Vector2(10f, 10f);
            position = new Vector2(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(300));
            keys = new Keys[3] { Keys.A, Keys.D, Keys.W };
            CreateCharacter(0, size, position, Color.Blue, "squares", keys);

            size = new Vector2(10f, 10f);
            position = new Vector2(ConvertUnits.ToSimUnits(500), ConvertUnits.ToSimUnits(300));
            keys = new Keys[3] { Keys.Left, Keys.Right, Keys.Up };
            CreateCharacter(1, size, position, Color.Red, "squares", keys);
        }

        private void CreateCharacter(int player, Vector2 size, Vector2 position, Color color, String texture, Keys[] keys)
        {
            if (player < bodyCount)
            {
                Vector2 convertedSize = ConvertUnits.ToSimUnits(size);

                bodys[player] = BodyFactory.CreateRectangle(world, convertedSize.X, convertedSize.Y, 13f, position);
                bodys[player].BodyType = BodyType.Dynamic;
                bodys[player].Friction = 1f;
                bodys[player].FixedRotation = false;

                jumpingSpeed[player] = 0;
                bodyHasContact[player] = false;
                bodys[player].OnCollision += OnCollision;
                bodys[player].OnSeparation += OnSeperation;

                for (int i = 0; i < 3; i++)
                {
                    controls[player,i] = keys[i];
                }

                speed[player] = 1f;
                jumpHeight[player] = bodys[player].Mass*4;                
                maxSpeed[player] = 3f;

                sprites[player] = new PhysicsSprite(bodys[player], "Graphics/" + texture, color, size);
            }
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

        private bool BottomCollision(Vector2 dir)
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

        private Vector2 GetContactNormal(Contact contact)
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
                if (fix1.Body == bodys[i])
                {
                    if (BottomCollision(GetContactNormal(contact)))
                    {
                        bodyHasContact[i] = true;
                        break;
                    }                    
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
                        if (tempEdge.Contact.IsTouching() && BottomCollision(GetContactNormal(tempEdge.Contact)))
                        {
                            bodyHasContact[i] = true;
                            break;
                        }
                        
                        tempEdge = tempEdge.Next;
                    }
                    /*if (bodyHasContact[i])
                    {
                        break;
                    }   */                 
                }
            }
        } 
       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

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

            foreach (PhysicsSprite p in level)
            {
                p.Update();
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

                spriteBatch.DrawString(calibri, ("FPS " + fps ), new Vector2(10, 5), Color.Yellow,
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
