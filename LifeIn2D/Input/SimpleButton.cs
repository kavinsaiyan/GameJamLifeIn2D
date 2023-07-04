using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SimplePhysics;

namespace LifeIn2D.Input
{
    public class SimpleButton
    {
        public Trigger trigger;

        public event System.Action OnClick;

        public SimpleButton(int width, int height, Vector2 position)
        {
            trigger = new Trigger(width, height, position);
        }

        public void Update(CustomMouse mouse)
        {
            trigger.Update();
            if (trigger.Contains(mouse.WindowPosition.ToVector2()) &&
                mouse.IsLeftButtonClicked())
            {
                OnClick?.Invoke();
            }
        }

        public void Draw(Sprites sprites)
        {
            trigger.Draw(sprites);
        }

        public void MoveTo(Vector2 amount)
        {
            trigger.MoveTo(amount);
        }
    }
}