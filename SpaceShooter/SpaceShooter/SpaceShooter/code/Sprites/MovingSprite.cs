using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SideScrollerTest.code.Sprites
{
    class MovingSprite : Sprite
    {
        public Vector2 Speed { set; get; }
        public Vector2 SpeedLimit { get; set; }

        public MovingSprite(Vector2 position, float rotation) : base(position, rotation)
        {
            this.Speed = new Vector2(0, 0);
            this.SpeedLimit = new Vector2(10, 10);
        }

        public MovingSprite(Vector2 position, float rotation, Vector2 speed, Vector2 speedLimit) : base(position, rotation)
        {
            this.Speed = speed;
            this.SpeedLimit = speedLimit;
        }

        public MovingSprite(Vector2 position, float rotation, Vector2 turnPoint, float scale, float layerDepth, Vector2 speed, Vector2 speedLimit)
            : base(position, rotation, turnPoint, scale, layerDepth)
        {
            this.Speed = speed;
            this.SpeedLimit = speedLimit;
        }

        public void Move()
        {
            float x = (float)(Speed.X);
            float y = (float)(-Speed.Y);

            x += Position.X;
            y += Position.Y;

            Position = new Vector2(x, y);
        }

        public void DecreaseXSpeed(float amount)
        {
            float x;
            if (Math.Abs((x = Speed.X - amount)) < SpeedLimit.X)
            {
                Speed = new Vector2(x, Speed.Y);
            }
            
        }

        public void DecreaseYSpeed(float amount)
        {
            float y;
            if (Math.Abs((y = Speed.Y - amount)) < SpeedLimit.Y)
            {
                Speed = new Vector2(Speed.X, y);
            }
        }

        public void IncreaseXSpeed(float amount)
        {
            DecreaseXSpeed(-amount);
        }

        public void IncreaseYSpeed(float amount)
        {
            DecreaseYSpeed(-amount);
        }

        public void SetXSpeed(float amount)
        {
           
            if (Math.Abs(amount) < SpeedLimit.X)
            {
                Speed = new Vector2(amount, Speed.Y);
            }
            else
            {
                Speed = new Vector2(SpeedLimit.X, Speed.Y);
            }
        }

        public void SetYSpeed(float amount)
        {
            if (Math.Abs(amount) < SpeedLimit.Y)
            {
                Speed = new Vector2(Speed.X, amount);
            }
            else
            {
                Speed = new Vector2(Speed.X, SpeedLimit.Y);
            }
            
        }

        public void IncreaseSpeedForward(float amount)
        {
            IncreaseXSpeed(amount);
            IncreaseYSpeed(amount);
            float speedFactor = Speed.Length();
            Speed = new Vector2((float)(speedFactor * Math.Sin(Rotation)), (float)(speedFactor * Math.Cos(Rotation)));
        }

        public void DecreaseSpeedForward(float amount)
        {
            IncreaseSpeedForward(-amount);
        }

        public void RotateRight(float amount)
        {
            IncreaseSpeedForward(0);
            Rotation += amount;
        }

        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
        public void HandleCollision()
        {
            this.Speed = new Vector2(-Speed.X, -Speed.Y);
            this.Move();
            this.Speed = new Vector2(0, 0);
        }
    }
    
}
