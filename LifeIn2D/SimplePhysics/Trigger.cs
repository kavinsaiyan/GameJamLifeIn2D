using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame;
namespace LifeIn2D.SimplePhysics
{
    public class Trigger
    {
        public RectangleF rect;
        public bool isEntered;

        public Trigger(RectangleF rectangleF)
        {
            rect = rectangleF;
        }

        #region Events
        public event System.Action OnEnter;
        public event System.Action OnExit;
        public event System.Action OnStay;
        #endregion

        public void Check(in RectangleF other)
        {
            if (rect.Intersects(other))
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

        public void Draw(Sprites sprites)
        {
            sprites.DrawRectangle(rect.Position, rect.Width, rect.Height, Color.Green);
        }
    }
}