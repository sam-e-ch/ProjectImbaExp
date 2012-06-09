using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace WindowsGame1.Graphics
{
    class Laser
    {
        private List<LaserShot> shots;
        private SpaceShip spaceShip;

        private Dictionary<ShotColor, Texture2D> textures;

        public float Heading { get; set; }
        public int FireRate { get; set; }
        public int ShotLifeTime { get; set; }

        private Stopwatch watch;
        
        public Laser(SpaceShip spaceShip)
        {
            this.shots = new List<LaserShot>();
            this.spaceShip = spaceShip;
            this.textures = new Dictionary<ShotColor, Texture2D>(3);
            this.watch = new Stopwatch();
            this.FireRate = 40;
            this.ShotLifeTime = 1500;
            watch.Start();
        }

        public void LoadContent(ContentManager cm)
        {
            textures[ShotColor.Red] = cm.Load<Texture2D>("images/laser_red");
            textures[ShotColor.Green] = cm.Load<Texture2D>("images/laser_green");
            textures[ShotColor.Blue] = cm.Load<Texture2D>("images/laser_blue");
        }

        public void Update(double dt)
        {
            shots.ForEach(shot => shot.Update(dt));
            shots.RemoveAll(shot => shot.LifeTime >= this.ShotLifeTime);
        }

        public void Draw(SpriteBatch sp)
        {
            shots.ForEach(shot => shot.Draw(sp, textures[shot.Color]));
        }

        public void Shoot()
        {
            if (watch.Elapsed.TotalMilliseconds >= 1000.0f / FireRate)
            {
                shots.Add(new LaserShot(ShotColor.Red, Heading - MathHelper.ToRadians(10.0f), spaceShip.Position));
                shots.Add(new LaserShot(ShotColor.Blue, Heading, spaceShip.Position));
                shots.Add(new LaserShot(ShotColor.Green, Heading + MathHelper.ToRadians(10.0f), spaceShip.Position));
                watch.Restart();
            }            
        }

        public int getShotCount()
        {
            return this.shots.Count;
        }

        private enum ShotColor { Red = 0, Green = 1, Blue = 2 };

        private class LaserShot
        {
            public const int SPEED = 1000;

            public float Rotation { get; set; }
            public Vector2 Position { get; set; }
            public ShotColor Color { get; set; }

            private Stopwatch watch;
            public long LifeTime { get { return watch.ElapsedMilliseconds; } }

            public LaserShot(ShotColor color, float direction, Vector2 position)
                : base()
            {
                Rotation = direction;
                Position = position;
                Color = color;
                watch = new Stopwatch();
                watch.Start();
            }

            public void Update(double dt)
            {
                Position += SPEED * (new Vector2((float)(Math.Sin(Rotation)), (float)(-Math.Cos(Rotation)))) * (float)dt;
            }

            public bool isInField(Rectangle r)
            {
                return (Position.X >= 0 && Position.X < r.Width && Position.Y >= 0 && Position.Y < r.Height);
            }

            public void Draw(SpriteBatch sp, Texture2D tex)
            {
                sp.Draw(tex, Position, null, Microsoft.Xna.Framework.Color.White, (float)this.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
