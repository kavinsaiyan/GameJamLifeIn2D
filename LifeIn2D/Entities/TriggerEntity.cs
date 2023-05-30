using System;
using SimplePhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
namespace LifeIn2D
{
    public enum EntityID
    {
        Brain, Kidney,
        Heart
    }
    public class TriggerEntity
    {
        public Trigger trigger;
        public Texture2D graphic;
        public EntityID entityID;

        public event System.Action<EntityID> OnEnter;
        public event System.Action<EntityID> OnExit;
        public event System.Action<EntityID> OnStay;

        public TriggerEntity(Vector2 position, Texture2D graphic, int width, int height)
        {
            trigger = new Trigger(width, height, position);
            this.graphic = graphic;

            trigger.OnEnter += () => OnEnter?.Invoke(entityID);
            trigger.OnExit += () => OnExit?.Invoke(entityID);
            trigger.OnStay += () => OnStay?.Invoke(entityID);
        }

        public void Check(Entity entity)
        {
            trigger.Check(entity.body.GetAABB());
        }

        public void Update()
        {
            trigger.Update();
        }

        public void Draw(Sprites sprites)
        {
            trigger.Draw(sprites);
            sprites.Draw(graphic, Vector2.Zero, trigger.position, Color.White);
        }
    }
}