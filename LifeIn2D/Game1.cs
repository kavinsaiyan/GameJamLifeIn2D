using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using SimplePhysics;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _audioManager = new AudioManager(Content);
            _inputManager = new InputManager();

            _gridManager = new GridManager();
            _gridManager.OnTileCreated += OnTileCreated;
            _gridManager.OnPathFound += OnPathFound;
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content);

            CustomMouse.Instance.Initialize(GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        private void OnPathFound()
        {
            _inputManager.RemoveAllButtons();
            _gridManager.Reset();
            _gridManager.grid = new int[,]
            {
                {11, 0 , 5 , 14, 14},
                {14, 14, 2 , 14, 14},
                {14, 14, 3 , 7, 14},
                {14, 14, 14, 1 , 14},
                {14, 14, 14, 3 , 12},
            };
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content);
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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprites = new Sprites(this);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CustomMouse.Instance.Update();
            CustomKeyboard.Instance.Update();
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.F))
                _gridManager.FindPath();
            _inputManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _gridManager.Draw(_sprites);
            _inputManager.Draw(_sprites); // drawing debug button outline
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}