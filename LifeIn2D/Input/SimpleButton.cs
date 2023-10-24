using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SimplePhysics;

namespace LifeIn2D.Input
{
    public class SimpleButton : IButton
    {
        public Trigger trigger;

        public event System.Action OnClick;

        public SimpleButton(int width, int height, Vector2 position)
        {
            trigger = new Trigger(width, height, position);
        }

        public void Update()
        {
            trigger.Update();
            // Logger.Log("trigger  min " + trigger.boundingBox.Min + " max " + trigger.boundingBox.Max);
            if (trigger.Contains(CustomMouse.Instance.WindowPosition) && CustomMouse.Instance.IsLeftButtonClicked())
            {
                OnClick?.Invoke();
            }
        }

        public void Draw(Sprites sprites)
        {
        }

        public void MoveTo(Vector2 amount)
        {
            trigger.MoveTo(amount);
        }
    }
}