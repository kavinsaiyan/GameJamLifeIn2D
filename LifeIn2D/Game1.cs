using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using SimplePhysics;
using System.Collections.Generic;
using System;
using LifeIn2D.Main;

namespace LifeIn2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Sprites _sprites;
        private PhysicsWorld _world;
        private Entity _player;
        private List<Entity> _entities;
        private List<TriggerEntity> _triggerEntities;
        private GridManager _gridManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new PhysicsWorld();
            _entities = new List<Entity>();
            _triggerEntities = new List<TriggerEntity>();
            _gridManager = new GridManager();
            _gridManager.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content);
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

            _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds, 4);

            Vector2 movement = Vector2.Zero;
            CustomKeyboard.Instance.Update();
            if (CustomKeyboard.Instance.IsKeyDown(Keys.W) || CustomKeyboard.Instance.IsKeyDown(Keys.Up)) { movement.Y++; }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.S) || CustomKeyboard.Instance.IsKeyDown(Keys.Down)) { movement.Y--; }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.A) || CustomKeyboard.Instance.IsKeyDown(Keys.Left)) { movement.X--; }
            if (CustomKeyboard.Instance.IsKeyDown(Keys.D) || CustomKeyboard.Instance.IsKeyDown(Keys.Right)) { movement.X++; }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _gridManager.Draw(_sprites);
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}