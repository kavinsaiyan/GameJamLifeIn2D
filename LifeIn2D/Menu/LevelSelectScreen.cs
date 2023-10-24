using System;
using System.Collections.Generic;
using System.Data;
using LifeIn2D.Input;
using LifeIn2D.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace LifeIn2D
{
    public enum LevelState { Locked, Playable }
    public class LevelSelectScreen
    {
        private int _levelsCount;
        private List<LevelCard> _levelCards = new List<LevelCard>();
        public event Action<int> OnLevelSelected;
        private LevelInfo _levelInfo;
        private bool _isOpen;
        public bool IsOpen => _isOpen;

        public LevelSelectScreen(LevelInfo levelInfo, int screenWidth, int screenHeight, ContentManager contentManager)
        {
            _levelInfo = levelInfo;
            _levelsCount = levelInfo.LevelDatas.Count;
            LevelSaveData.Instance.Init();
            LevelSaveData.Instance.InitializeSaveItems(_levelsCount);

            //load the textures and font
            Texture2D buttonTexture = contentManager.Load<Texture2D>("Buttons/Level_Button");
            Texture2D lockTexture = contentManager.Load<Texture2D>("Buttons/Lock");
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/JupiteroidRegular-Rpj6V");

            //create the cards
            int halfScreenWidth = screenWidth / 2;
            int halfScreenHeight = screenHeight / 2;
            int buttonSize = buttonTexture.Width / 2 + buttonTexture.Height / 2;
            int rowCount = _levelsCount / 2;
            int rows = rowCount;
            Vector2 cardStartPos = new Vector2(halfScreenWidth - buttonSize * _levelsCount / 2, halfScreenHeight + buttonSize);
            float x = 0, y = 0;
            for (int i = 0; i < _levelsCount; i++)
            {
                Vector2 pos = new Vector2(cardStartPos.X + x, cardStartPos.Y + y);

                TexturedButton texturedButton = new TexturedButton(buttonTexture.Width, buttonTexture.Height, pos, buttonTexture);
                int k = i + 1;
                texturedButton.OnClick += delegate { OnLevelCardClicked(k); };

                LevelCard levelCard = new LevelCard(lockTexture, font, texturedButton, i + 1);
                if (LevelSaveData.Instance.TryGetSaveItem(i + 1, out LevelSaveItem levelSaveItem))
                    levelCard.LevelState = levelSaveItem.levelState;
                else
                    Logger.Instance.Log("save data not found for " + (i + 1));
                _levelCards.Add(levelCard);

                x += buttonSize * 2;
                if (i >= rows)
                {
                    rows += rowCount;
                    x = 0;
                    y -= buttonSize * 2;
                }
            }
        }

        public void Open(InputManager inputManager)
        {
            _isOpen = true;
        }
        public void Close(InputManager inputManager)
        {
            _isOpen = false;
        }
        private void OnLevelCardClicked(int levelNumber)
        {
            OnLevelSelected?.Invoke(levelNumber);
        }
        public void Update()
        {
            if (_isOpen == false)
                return;
            for (int i = 0; i < _levelsCount; i++)
            {
                _levelCards[i].Update();
            }
        }

        public void Draw(Sprites sprites)
        {
            if (_isOpen == false)
                return;
            for (int i = 0; i < _levelsCount; i++)
            {
                _levelCards[i].Draw(sprites);
            }
        }
    }
}