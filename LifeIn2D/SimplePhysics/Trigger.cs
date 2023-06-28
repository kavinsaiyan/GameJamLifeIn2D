using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame;
using LifeIn2D;
using System;

namespace SimplePhysics
{
    public class Trigger
    {
        public AABB boundingBox;
        public int width;
        public int height;
        public Vector2 position;
        public bool isEntered;
        private bool aabbUpdateRequired = false;
        #region Events
        public event System.Action OnEnter;
        public event System.Action OnExit;
        public event System.Action OnStay;
        #endregion

        public Trigger(int width, int height, Vector2 position)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            aabbUpdateRequired = true;
            Update();
        }

        public void Update()
        {
            if (aabbUpdateRequired)
            {
                Vector2 dim = new Vector2(width, height);
                boundingBox = new AABB(position, position + dim);
                aabbUpdateRequired = false;
            }
        }


        public void Check(in AABB other)
        {
            if (Collisions.Collide(boundingBox, other))
            {
                if (isEntered == false)
                {
                    isEntered = true;
                    OnEnter?.Invoke();
                    return;
                }
                OnStay?.Invoke();
            }
            else
            {
                if (isEntered == true)
                    OnExit?.Invoke();
                isEntered = false;
            }
        }

        public void Move(Vector2 amount)
        {
            position += amount;
            aabbUpdateRequired = true;
        }

        public void Draw(Sprites sprites)
        {
            sprites.DrawRectangle(position, width, height, Color.Green);
        }
    }
}