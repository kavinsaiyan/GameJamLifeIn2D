﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using System.Collections.Generic;
using System;
using LifeIn2D.Main;
using MonoGame.Extended;
using LifeIn2D.Audio;
using LifeIn2D.Entities;

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
        private LevelLoader _levelLoader;
        private OrganTileManager _organTileManager;
        private TileScaleAnimation _tileScaleAnimation;

        private Timer _timer;
        private TextDisplayAction _displayAction;
        private WaitForDelayAction _waitForDelayAction;
        private SpriteFont _jupiteroidFont;
        private bool _displayGame = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _inputManager = new InputManager();

            _levelLoader = new LevelLoader();
            _levelLoader.currentLevel = 1;
            _levelLoader.Load();

            _gridManager = new GridManager();
            _gridManager.OnTileCreated += OnTileCreated;
            _gridManager.OnPathFound += OnPathFound;

            CustomMouse.Instance.Initialize(GraphicsDevice.Viewport.Height);

            _timer = new Timer();

            _organTileManager = new OrganTileManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprites = new Sprites(this);
            _audioManager = new AudioManager(Content);
            _jupiteroidFont = Content.Load<SpriteFont>("Fonts/JupiteroidRegular-Rpj6V");
            InitializeLevelText();
        }

        private void InitalizeWaitForDelay()
        {
            _waitForDelayAction = new WaitForDelayAction(2);
            _waitForDelayAction.OnComplete += OnDelayComplete;
            _timer.AddAction(_waitForDelayAction);
        }

        private void OnDelayComplete()
        {
            _gridManager.Reset();
            InitializeLevelText();
            _displayGame = false;
        }

        private void InitializeLevelText()
        {
            _displayAction = new TextDisplayAction(2, "Level " + _levelLoader.currentLevel, _jupiteroidFont,
                            new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 + 50),
                            Color.Black);
            _displayAction.OnComplete += OnTextDisplayComplete;
            _timer.AddAction(_displayAction);
        }

        private void OnTextDisplayComplete()
        {
            _displayGame = true;
            _levelLoader.Load();

            _gridManager.grid = _levelLoader.grid;
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content
                                    , _levelLoader.destinationsCount);
            _gridManager.FindPath();

            for (int i = 0; i < _gridManager.tileGrid.GetLength(0); i++)
               for(int j =0; j< _gridManager.tileGrid.GetLength(1); j++)
                    if(_gridManager.tileGrid[i,j].Id == TileID.Heart)
                    {
                        _tileScaleAnimation = new TileScaleAnimation(_gridManager.tileGrid[i,j]);
                        break;
                    }

            InitalizeOrganTileManager();
        }

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
            _levelLoader.currentLevel++;
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
            if (_displayGame)
            {
                _gridManager.Draw(_sprites);
                _organTileManager.Draw(_sprites);
                // _inputManager.Draw(_sprites); // drawing debug button outline
            }
            _displayAction.Draw(_sprites);
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}