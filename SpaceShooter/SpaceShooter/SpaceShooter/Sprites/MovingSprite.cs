using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Sprites
{
    class MovingSprite : Sprite
    {
        public float Speed { set; get; }
        public float SpeedLimit { get; set; }

        public MovingSprite(Vector2 position, float rotation) : base(position, rotation)
        {
            this.Speed =3f;
            this.SpeedLimit = 10f;
        }

        public MovingSprite(Vector2 position, float rotation, float speed, float speedLimit) : base(position, rotation)
        {
            this.Speed = speed;
            this.SpeedLimit = speedLimit;
        }

        public MovingSprite(Vector2 position, float rotation, Vector2 turnPoint, float scale, float layerDepth, float speed, float speedLimit)
            : base(position, rotation, turnPoint, scale, layerDepth)
        {
            this.Speed = speed;
            this.SpeedLimit = speedLimit;
        }

        public void MoveRight(int amount)
        {            
            Position = new Vector2(Position.X + amount, Position.Y);
        }

        public void MoveLeft(int amount)
        {
            this.MoveRight(-amount);
        }

        public void MoveDown(int amount)
        {
            Position = new Vector2(Position.X, Position.Y + amount);
        }

        public void MoveUp(int amount)
        {
            this.MoveDown(-amount);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + Speed, Position.Y);
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - Speed, Position.Y);
        }

        public void MoveDown()
        {
            Position = new Vector2(Position.X, Position.Y + Speed);
        }

        public void MoveUp()
        {
            Position = new Vector2(Position.X, Position.Y - Speed);
        }

        public void RotateRight(float amount)
        {
            Rotation += amount;
        }

        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }

        public void MoveForward()
        {
            float x = Position.X + (float)(Speed * Math.Sin(Rotation));
            float y = Position.Y - (float)(Speed * Math.Cos(Rotation));
            Position = new Vector2(x, y);

        }

        public void MoveBackward()
        {
            float x = Position.X - (float)(Speed * Math.Sin(Rotation));
            float y = Position.Y + (float)(Speed * Math.Cos(Rotation));
            Position = new Vector2(x, y);
        }

        public void IncreaseSpeed()
        {
            if (Speed <= SpeedLimit)
            {
                this.Speed++;
            }
        }

        public void DecreaseSpeed()
        {
            if (Speed > 0)
            {
                this.Speed--;
            }
        }
        
        
    }
    
}
