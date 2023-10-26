using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using LifeIn2D.Main;
using LifeIn2D.Audio;
using LifeIn2D.Entities;
using LifeIn2D.Menu;

namespace LifeIn2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Sprites _sprites;
        private GridManager _gridManager;
        private InputManager _inputManager;
        private AudioManager _audioManager;
        private LevelInfo _levelInfo;
        private int _currentLevel;
        private Texture2D _backGround;
        private OrganTileManager _organTileManager;
        private TileScaleAnimation _tileScaleAnimation;

        private Timer _timer;
        private TextDisplayAction _displayAction;
        private WaitForDelayAction _waitForDelayAction;
        private SpriteFont _jupiteroidFont;
        private GameState _gameState;
        private HomeScreen _homeScreen;
        private LevelSelectScreen _levelSelectScreen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _inputManager = new InputManager();

            _gridManager = new GridManager();
            _gridManager.OnTileCreated += OnTileCreated;
            _gridManager.OnPathFound += OnPathFound;

            CustomMouse.Instance.Initialize(GraphicsDevice.Viewport.Height);

            _timer = new Timer();

            _organTileManager = new OrganTileManager();

            LevelSaveData.Instance.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sprites = new Sprites(this);
            _audioManager = new AudioManager(Content);
            _jupiteroidFont = Content.Load<SpriteFont>("Fonts/JupiteroidRegular-Rpj6V");

            _levelInfo = new LevelInfo();
            _levelSelectScreen = new LevelSelectScreen(_levelInfo, GraphicsDevice.Viewport.Width,
                                                              GraphicsDevice.Viewport.Height, Content);
            _levelSelectScreen.OnLevelSelected += OnLevelSelected;
            _currentLevel = 1;

            _gameState = GameState.HomeScreen;
            _homeScreen = new HomeScreen(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _homeScreen.OnPlayButtonClicked += OnPlayButtonClicked;
            _homeScreen.OnExitButtonClicked += Exit;
            _homeScreen.Open(_inputManager);

            _backGround = Content.Load<Texture2D>("LifeIn2D_BG");
        }
        #region End Level Delay
        private void InitalizeWaitForDelay()
        {
            _waitForDelayAction = new WaitForDelayAction(2);
            _waitForDelayAction.OnComplete += OnDelayComplete;
            _timer.AddAction(_waitForDelayAction);
        }

        private void OnDelayComplete()
        {
            _gridManager.Reset();
            if (_currentLevel + 1 >= _levelInfo.LevelDatas.Count)
            {
                InitializeLevelSelectScreen();
                return;
            }

            LevelSaveData.Instance.Load();
            if (LevelSaveData.Instance.TryGetSaveItem(_currentLevel + 1, out LevelSaveItem nextLevelSaveItem))
            {
                if (nextLevelSaveItem.levelState == LevelState.Playable)
                {
                    InitializeLevelSelectScreen();
                    return;
                }
                else
                {
                    nextLevelSaveItem.levelState = LevelState.Playable;
                    LevelSaveData.Instance.Save();
                    _currentLevel++;
                    InitializeLevelText();
                }
            }
        }
        #endregion
        #region Level Number Text
        private void InitializeLevelText()
        {
            _displayAction = new TextDisplayAction(2, "Level " + _currentLevel, _jupiteroidFont,
                            new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 + 50),
                            Color.Black);
            _displayAction.OnComplete += OnTextDisplayComplete;
            _timer.AddAction(_displayAction);
            _gameState = GameState.LevelTextDisplay;
        }

        private void OnTextDisplayComplete()
        {
            _gameState = GameState.GamePlaying;

            if (_currentLevel - 1 < 0 || _currentLevel - 1 >= _levelInfo.LevelDatas.Count)
            {
                Logger.Instance.LogError("[Game1.cs/OnTextDisplayComplete]: _current level is out of bounds!");
                return;
            }

            _gridManager.grid = _levelInfo.LevelDatas[_currentLevel - 1].grid;
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content
                                    , _levelInfo.LevelDatas[_currentLevel - 1].destinationsCount);
            _gridManager.FindPath();

            for (int i = 0; i < _gridManager.tileGrid.GetLength(0); i++)
                for (int j = 0; j < _gridManager.tileGrid.GetLength(1); j++)
                    if (_gridManager.tileGrid[i, j].Id == TileID.Heart)
                    {
                        _tileScaleAnimation = new TileScaleAnimation(_gridManager.tileGrid[i, j]);
                        break;
                    }

            InitalizeOrganTileManager();
        }
        #endregion

        private void InitalizeOrganTileManager()
        {
            _organTileManager.Initialize();
            for (int i = 0; i < _gridManager.tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < _gridManager.tileGrid.GetLength(1); j++)
                {
                    Tile tile = _gridManager.tileGrid[i, j];
                    if (tile.Id != TileID.None)
                        _organTileManager.CreateOrganIfPossible(tile, Content, out OrganTile organTile);
                }
            }
        }

        private void OnPathFound()
        {
            _inputManager.RemoveAllButtons();
            InitalizeWaitForDelay();
        }

        private void OnTileCreated(Tile tile)
        {
            if (tile.Id == TileID.None)
                return;

            SimpleButton button = new SimpleButton(tile.Width, tile.Height, tile.Position - tile.Origin);
            button.OnClick += tile.Rotate;
            _inputManager.AddButton(button);

            button.OnClick += _audioManager.PlayClickSound;
            button.OnClick += _gridManager.FindPath;
        }
        #region Home screen
        private void OnPlayButtonClicked()
        {
            _homeScreen.Close(_inputManager);
            InitializeLevelSelectScreen();
        }
        #endregion

        #region Level Select Screen    
        private void InitializeLevelSelectScreen()
        {
            _gameState = GameState.LevelSelectionScreen;
            _levelSelectScreen.Open(_inputManager);
        }

        private void OnLevelSelected(int level)
        {
            _levelSelectScreen.Close(_inputManager);
            _currentLevel = level;
            InitializeLevelText();
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CustomMouse.Instance.Update();
            CustomKeyboard.Instance.Update();
            _inputManager.Update();
            _timer.Update(gameTime.ElapsedGameTime.TotalSeconds);
            switch (_gameState)
            {
                case GameState.LevelTextDisplay:

                    break;
                case GameState.GamePlaying:
                    _tileScaleAnimation?.Update(gameTime);
                    if (CustomKeyboard.Instance.IsKeyClicked(Keys.F))
                        _gridManager.FindPath();
                    _gridManager.Update(gameTime);
                    break;
                case GameState.LevelSelectionScreen:
                    _levelSelectScreen.Update();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _sprites.Draw(_backGround, new Vector2(_backGround.Width / 2, _backGround.Height / 2),
                        new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White,);
            switch (_gameState)
            {
                case GameState.HomeScreen:
                    _inputManager.Draw(_sprites);
                    break;
                case GameState.LevelTextDisplay:
                    _displayAction.Draw(_sprites);
                    break;
                case GameState.GamePlaying:
                    _inputManager.Draw(_sprites);
                    _gridManager.Draw(_sprites);
                    _organTileManager.Draw(_sprites);
                    break;
                case GameState.LevelSelectionScreen:
                    _levelSelectScreen.Draw(_sprites);
                    break;
            }
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}