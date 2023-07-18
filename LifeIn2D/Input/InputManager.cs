using System.Collections.Generic;

namespace LifeIn2D.Input
{
    public class InputManager
    {
        private List<SimpleButton> _simpleButtons;

        public InputManager()
        {
            _simpleButtons = new List<SimpleButton>();
        }

        public void AddButton(SimpleButton simpleButton)
        {
            _simpleButtons.Add(simpleButton);
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