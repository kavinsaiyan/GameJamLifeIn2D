using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using SimplePhysics;
using System.Collections.Generic;
using System;
using LifeIn2D.Main;
using MonoGame.Extended;

namespace LifeIn2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Sprites _sprites;
        private GridManager _gridManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _gridManager = new GridManager();
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content);

            CustomMouse.Instance.Initialize(GraphicsDevice.Viewport.Height);
            base.Initialize();
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

            Vector2 movement = Vector2.Zero;
            CustomKeyboard.Instance.Update();
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.W) || CustomKeyboard.Instance.IsKeyClicked(Keys.Up)) { movement.Y++; _gridManager.tileGrid[0, 1].Rotate(); }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.S) || CustomKeyboard.Instance.IsKeyDown(Keys.Down)) { movement.Y--; }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.A) || CustomKeyboard.Instance.IsKeyDown(Keys.Left)) { movement.X--; }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.D) || CustomKeyboard.Instance.IsKeyDown(Keys.Right)) { movement.X++; }
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.K))
            { _gridManager.FindPath(); }
            CustomMouse.Instance.Update();
            _gridManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _gridManager.Draw(_sprites);
            _sprites.DrawCircle(CustomMouse.Instance.WindowPosition, 4f, 12, Color.White);
            Vector2 ingameMousePos = new Vector2(CustomMouse.Instance.WindowPosition.X, GraphicsDevice.Viewport.Height - CustomMouse.Instance.WindowPosition.Y);
            _sprites.DrawCircle(ingameMousePos, 4f, 12, Color.White);

            _sprites.End();

            base.Draw(gameTime);
        }
    }
}