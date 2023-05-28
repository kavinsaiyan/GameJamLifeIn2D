using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SimplePhysics;

namespace LifeIn2D
{
    public class Entity
    {
        public Rigidbody body;
        public Texture2D graphic;
        public Vector2 spriteScale = Vector2.One;
        private Vector2 _grahicOffset;
        public RectangleF rect;
        public Entity(Rigidbody body, Texture2D graphic)
        {
            this.body = body;
            this.graphic = graphic;
        }

        public Entity(PhysicsWorld world, float radius, bool isStatic, Vector2 position, Texture2D graphic)
        {
            if (Rigidbody.CreateCircleBody(radius, 2f, isStatic, 0.6f, out Rigidbody body, out string errorMessage) == false)
                throw new Exception(errorMessage);
            body.MoveTo(position);
            this.body = body;
            this.graphic = graphic;
            _grahicOffset = new Vector2(body.Radius / 2f, body.Radius / 2f);
            rect = new RectangleF(position.X - _grahicOffset.X, position.Y - _grahicOffset.Y, radius, radius);
            world.AddBody(body);
        }
        public Entity(PhysicsWorld world, float width, float height, bool isStatic, Vector2 position, Texture2D graphic)
        {
            if (Rigidbody.CreateBoxBody(width, height, 2f, isStatic, 0.6f, out Rigidbody body, out string errorMessage) == false)
                throw new Exception(errorMessage);
            body.MoveTo(position);
            this.body = body;
            this.graphic = graphic;
            _grahicOffset = new Vector2(body.Width / 2f, body.Height / 2f);
            rect = new RectangleF(position.X - _grahicOffset.X, position.Y - _grahicOffset.Y, width, height);
            world.AddBody(body);
        }

        public void Update()
        {
            rect.Position = body.Position - _grahicOffset;
        }
        public void Draw(Sprites sprites)
        {
            Vector2 position = this.body.Position - _grahicOffset;
            if (body.ShapeType is ShapeType.Box)
            {
                if (graphic != null)
                    sprites.Draw(graphic, null, Vector2.Zero, position, 0, spriteScale, Color.White);
                sprites.DrawRectangle(position, body.Width, body.Height, Color.White);
            }
            else if (body.ShapeType is ShapeType.Circle)
                sprites.DrawCircle(position, body.Radius, 32, Color.White);
        }
    }
}