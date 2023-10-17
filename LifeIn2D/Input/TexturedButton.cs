using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SimplePhysics;

namespace LifeIn2D.Input
{
    public class TexturedButton : IButton
    {
        public Trigger trigger;
        public event System.Action OnClick;
        public event System.Action OnHover;
        private Texture2D _texture;
        private Color _color;

        public TexturedButton(int width, int height, Vector2 position, Texture2D texture)
        {
            _texture = texture;
            _color = Color.White;
            trigger = new Trigger(width, height, position);
        }

        public void Update()
        {
            trigger.Update();
            _color = Color.White;
            // Logger.Log("trigger  min " + trigger.boundingBox.Min + " max " + trigger.boundingBox.Max);
            if (trigger.Contains(CustomMouse.Instance.WindowPosition))
            {
                OnHover?.Invoke();
                _color = Color.LightGray;
                if (CustomMouse.Instance.IsLeftButtonClicked())
                {
                    OnClick?.Invoke();
                    _color = Color.Gray;
                }
            }
        }

        public void Draw(Sprites sprites)
        {
            // trigger.Draw(sprites); // for debugging only
            sprites.Draw(_texture, new Vector2(_texture.Width / 2, _texture.Height / 2), trigger.position, _color);
        }

        public void MoveTo(Vector2 amount)
        {
            trigger.MoveTo(amount);
        }
    }
}