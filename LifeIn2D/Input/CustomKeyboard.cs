using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LifeIn2D.Input
{
    public sealed class CustomKeyboard
    {
        private static readonly Lazy<CustomKeyboard> lazy = new Lazy<CustomKeyboard>(() => new CustomKeyboard());
        public static CustomKeyboard Instance
        {
            get { return lazy.Value; }
        }

        private KeyboardState _prevKeyboardState;
        private KeyboardState _currentKeyboardState;

        public CustomKeyboard()
        {
            _prevKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            _currentKeyboardState = _prevKeyboardState;
        }

        public void Update()
        {
            _prevKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        public bool IsKeyDown(Keys keys)
        {
            return _currentKeyboardState.IsKeyDown(keys);
        }

        public bool IsKeyClicked(Keys keys)
        {
            return _currentKeyboardState.IsKeyDown(keys) && !_prevKeyboardState.IsKeyDown(keys);
        }
    }
}