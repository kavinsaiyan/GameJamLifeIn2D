using System;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace LifeIn2D.Menu
{
    public class HomeScreen
    {
        private TexturedButton _playButton;
        private TexturedButton _exitButton;
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set => _isOpen = value; }
        public event Action OnPlayButtonClicked;
        public event Action OnExitButtonClicked;
        public HomeScreen(ContentManager contentManager, int screenWidth, int screenHeight)
        {
            Texture2D playTexture = contentManager.Load<Texture2D>("Buttons/PlayButton");
            Vector2 playerButtonPos = new Vector2(screenWidth / 2 - playTexture.Width / 2, screenHeight / 2 + playTexture.Height / 2);
            _playButton = new TexturedButton(playTexture.Width, playTexture.Height, playerButtonPos, playTexture);
            Texture2D exitTexture = contentManager.Load<Texture2D>("Buttons/ExitButton");
            Vector2 exitButtonPos = new Vector2(screenWidth / 2 - exitTexture.Width / 2, screenHeight / 2 - exitTexture.Height);
            _exitButton = new TexturedButton(exitTexture.Width, exitTexture.Height, exitButtonPos, exitTexture);
            _isOpen = true;
            _playButton.OnClick += PlayButtonClicked;
            _exitButton.OnClick += ExitButtonClicked;
        }

        private void PlayButtonClicked()
        {
            OnPlayButtonClicked?.Invoke();
        }

        private void ExitButtonClicked()
        {
            OnExitButtonClicked?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            if (_isOpen)
            {
                _playButton.Update();
                _exitButton.Update();
            }
        }

        public void Draw(Sprites sprites)
        {
            if (_isOpen)
            {
                _playButton.Draw(sprites);
                _exitButton.Draw(sprites);
            }
        }
    }
}