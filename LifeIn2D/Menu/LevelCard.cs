using System;
using System.Net.Mime;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Menu
{
    public class LevelCard
    {
        private Texture2D _lockIcon;
        private SpriteFont _spriteFont;
        private TexturedButton _button;
        public TexturedButton Button => _button;
        private int _levelNumber;

        private LevelState _levelState;
        public LevelState LevelState
        {
            get => _levelState;
            set
            {
                _levelState = value;
            }
        }

        public LevelCard(Texture2D lockIcon, SpriteFont spriteFont, TexturedButton button, int levelNumber)
        {
            _lockIcon = lockIcon;
            _spriteFont = spriteFont;
            _button = button;
            _levelNumber = levelNumber;
        }

        public void Update()
        {
            switch(_levelState)
            {
                case LevelState.Playable:
                    _button.Update();
                    break;
            }
        }

        public void Draw(Sprites sprites)
        {
            _button.Draw(sprites);
            switch (_levelState)
            {
                case LevelState.Playable:
                    sprites.DrawString(_spriteFont, _levelNumber.ToString(), _button.trigger.position + new Vector2(_button.trigger.width/4,0), Color.Black, 3);
                    break;
                case LevelState.Locked:
                    sprites.Draw(_lockIcon,Vector2.Zero, _button.trigger.position,Color.White);
                    break;
            }
        }
    }
}