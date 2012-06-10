using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceShooter.Sprites
{
    class Sprite
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        private Texture2D texture;
        public Vector2 TurnPoint { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }

        public Color[] ColorData { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Sprite(Vector2 position, float rotation, Vector2 turnPoint, float scale, float layerDepth)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.TurnPoint = turnPoint;
            this.Scale = scale;
            this.LayerDepth = layerDepth;
        }

        public Sprite(Vector2 position, float rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.TurnPoint = new Vector2(0, 0);
            this.Scale = 1.0f;
            this.LayerDepth = 1.0f;
        }

        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            texture = theContentManager.Load<Texture2D>(textureName);
            this.Width = texture.Width;
            this.Height = texture.Height;
            ColorData = new Color[Width * Height];
            texture.GetData(ColorData);
            TurnPoint = new Vector2(Width / 2, Height / 2);
        }

        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, TurnPoint, Scale, SpriteEffects.None, LayerDepth);
        }

        public Boolean InField(int x, int y)
        {
            Rectangle rect = new Rectangle(0,0,Width,Height);
            return (Position.X >= rect.Width/2 && Position.X < x-rect.Width/2 && Position.Y >= rect.Height/2 && Position.Y < y-rect.Height/2);
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(new Vector3(-TurnPoint, 0.0f)) *
                    Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0.0f));
        }

        public bool IntersectPixels(Sprite other)
        {

            Matrix transformA = this.GetTransform();
            int widthA = this.Width;
            int heightA = this.Height;
            Color[] dataA = this.ColorData;
            Matrix transformB = other.GetTransform();
            int widthB = other.Width;
            int heightB = other.Height;
            Color[] dataB = other.ColorData;
            Rectangle thisRectangle = this.CalculateBoundingRectangle();
            Rectangle otherRectangle = other.CalculateBoundingRectangle();

            if (thisRectangle.Intersects(otherRectangle))
            {
                // Calculate a matrix which transforms from A's local space into
                // world space and then into B's local space
                Matrix transformAToB = transformA * Matrix.Invert(transformB);

                // When a point moves in A's local space, it moves in B's local space with a
                // fixed direction and distance proportional to the movement in A.
                // This algorithm steps through A one pixel at a time along A's X and Y axes
                // Calculate the analogous steps in B:
                Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
                Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

                // Calculate the top left corner of A in B's local space
                // This variable will be reused to keep track of the start of each row
                Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

                // For each row of pixels in A
                for (int yA = 0; yA < heightA; yA++)
                {
                    // Start at the beginning of the row
                    Vector2 posInB = yPosInB;

                    // For each pixel in this row
                    for (int xA = 0; xA < widthA; xA++)
                    {
                        // Round to the nearest pixel
                        int xB = (int)Math.Round(posInB.X);
                        int yB = (int)Math.Round(posInB.Y);

                        // If the pixel lies within the bounds of B
                        if (0 <= xB && xB < widthB &&
                            0 <= yB && yB < heightB)
                        {
                            // Get the colors of the overlapping pixels
                            Color colorA = dataA[xA + yA * widthA];
                            Color colorB = dataB[xB + yB * widthB];

                            // If both pixels are not completely transparent,
                            if (colorA.A != 0 && colorB.A != 0)
                            {
                                // then an intersection has been found
                                return true;
                            }
                        }

                        // Move to the next pixel in the row
                        posInB += stepX;
                    }

                    // Move to the next row
                    yPosInB += stepY;
                }
            }
            // No intersection found
            return false;
        }

        public Rectangle CalculateBoundingRectangle()
        {
            Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
            Matrix transform = this.GetTransform();

            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

    }
}
