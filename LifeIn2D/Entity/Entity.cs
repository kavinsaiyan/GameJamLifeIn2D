using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePhysics;

namespace LifeIn2D.Entity
{
    public class Entity
    {
        public Rigidbody body;
        public Texture2D graphic;
        public Entity(Rigidbody body, Texture2D graphic)
        {
            this.body = body;
            this.graphic = graphic;
        }

        public Entity(PhysicsWorld world, float radius, bool isStatic, Vector2 position)
        {
            if (Rigidbody.CreateCircleBody(radius, 2f, false, 0.6f, out Rigidbody body, out string errorMessage) == false)
                throw new Exception(errorMessage);
            body.MoveTo(position);
            this.body = body;
            world.AddBody(body);
        }
        public Entity(PhysicsWorld world, float width, float height, bool isStatic, Vector2 position)
        {
            if (Rigidbody.CreateBoxBody(width, height, 2f, false, 0.6f, out Rigidbody body, out string errorMessage) == false)
                throw new Exception(errorMessage);
            body.MoveTo(position);
            this.body = body;
            world.AddBody(body);
        }
        public void Draw(Sprites sprites)
        {
            Vector2 position = this.body.Position;
            sprites.Draw(graphic, Vector2.Zero, body.Position, Color.White);
            if (body.ShapeType is ShapeType.Box)
                sprites.DrawRectangle(position, body.Width, body.Height, Color.White);
            else if (body.ShapeType is ShapeType.Circle)
                sprites.DrawCircle(position, body.Radius, 32, Color.White);
        }
    }
}