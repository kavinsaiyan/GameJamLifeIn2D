using System.Collections.Generic;

namespace LifeIn2D.Input
{
    public class InputManager
    {
        public List<SimpleButton> simpleButtons;

        public static InputManager instance;
        public InputManager()
        {
            simpleButtons = new List<SimpleButton>();
            instance = this;
        }
    }
}