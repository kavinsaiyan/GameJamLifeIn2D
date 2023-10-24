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
        private SpriteBatch _spriteBatch;
        private Sprites _sprites;
        private GridManager _gridManager;

        private InputManager _inputManager;
        private AudioManager _audioManager;
        private LevelInfo _levelInfo;
        private int _currentLevel;
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

            _levelInfo = new LevelInfo();
            _levelSelectScreen = new LevelSelectScreen(_levelInfo, GraphicsDevice.Viewport.Width,
                                                              GraphicsDevice.Viewport.Height, Content);
            _levelSelectScreen.OnLevelSelected += OnLevelSelected;
            _currentLevel = 1;

            _gridManager = new GridManager();
            _gridManager.OnTileCreated += OnTileCreated;
            _gridManager.OnPathFound += OnPathFound;

            CustomMouse.Instance.Initialize(GraphicsDevice.Viewport.Height);

            _timer = new Timer();

            _organTileManager = new OrganTileManager();


            _gameState = GameState.HomeScreen;
            _homeScreen = new HomeScreen(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _homeScreen.OnPlayButtonClicked += OnPlayButtonClicked;
            _homeScreen.OnExitButtonClicked += Exit;
            _homeScreen.Open(_inputManager);
            base.Initialize();

            LevelSaveData.Instance.Init();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprites = new Sprites(this);
            _audioManager = new AudioManager(Content);
            _jupiteroidFont = Content.Load<SpriteFont>("Fonts/JupiteroidRegular-Rpj6V");
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

            _gridManager.grid = _levelInfo.LevelDatas[_currentLevel].grid;
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content
                                    , _levelInfo.LevelDatas[_currentLevel].destinationsCount);
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
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.F))
                _gridManager.FindPath();
            _gridManager.Update(gameTime);
            _inputManager.Update();
            _timer.Update(gameTime.ElapsedGameTime.TotalSeconds);
            _tileScaleAnimation?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            switch (_gameState)
            {
                case GameState.LevelTextDisplay:
                    _displayAction.Draw(_sprites);
                    break;
                case GameState.GamePlaying:
                    _gridManager.Draw(_sprites);
                    _organTileManager.Draw(_sprites);
                    _inputManager.Draw(_sprites);
                    break;
            }
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}