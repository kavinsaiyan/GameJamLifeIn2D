using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LifeIn2D.Input
{
    public sealed class CustomMouse
    {
        private static readonly Lazy<CustomMouse> lazy = new Lazy<CustomMouse>(() => new CustomMouse());
        public static CustomMouse Instance
        {
            get { return lazy.Value; }
        }
        public Point MouseWindowPosition
        {
            get
            {
                return _currentMouseState.Position;
            }
        }
        private MouseState _prevMouseState;
        private MouseState _currentMouseState;

        public Point WindowPosition
        {
            get
            {
                return _currentMouseState.Position;
            }
        }

        public CustomMouse()
        {
            _prevMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _currentMouseState = _prevMouseState;
        }

        public void Update()
        {
            _prevMouseState = _currentMouseState;
            _currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public bool IsLeftButtonDown()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightButtonDown()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsMiddleButtonDown()
        {
            return _currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsRightButtonClicked()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _prevMouseState.RightButton == ButtonState.Released;
        }

        public bool IsMiddleButtonClicked()
        {
            return _currentMouseState.MiddleButton == ButtonState.Pressed && _prevMouseState.MiddleButton == ButtonState.Released;
        }
    }
}