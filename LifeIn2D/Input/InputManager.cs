using System.Collections.Generic;

namespace LifeIn2D.Input
{
    public class InputManager
    {
        private List<IButton> _simpleButtons;

        public InputManager()
        {
            _simpleButtons = new List<IButton>();
        }

        public void AddButton(IButton button)
        {
            _simpleButtons.Add(button);
        }

        public void RemoveAllButtons()
        {
            _simpleButtons.Clear();
        }

        public void Update()
        {
            for (int i = 0; i < _simpleButtons.Count; i++)
            {
                _simpleButtons[i].Update();
            }
        }

        public void Draw(Sprites sprites)
        {
            for (int i = 0; i < _simpleButtons.Count; i++)
            {
                _simpleButtons[i].Draw(sprites);
            }
        }
    }
}