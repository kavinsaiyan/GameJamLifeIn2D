using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
using SimplePhysics;
using System.Collections.Generic;
using System;

namespace LifeIn2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Sprites _sprites;
        private Texture2D _simpleHeart;
        private PhysicsWorld _world;
        private Entity _player;
        private List<Entity> _entities;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprites = new Sprites(this);
            _simpleHeart = Content.Load<Texture2D>("SimpleHeart");

            Texture2D playerSprite = Content.Load<Texture2D>("Player");
            _player = new Entity(_world, 32, 32, false, new Vector2(100, 100), playerSprite);
            _player.spriteScale = Vector2.One / 2f;
            _player.body.UseGravity = false;
            _entities.Add(_player);
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
            // Console.WriteLine("movmet : " + movement + " INv mass " + _player.body.InvMass);
            _player.body.AddForce(movement * 100000 / 0.016f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _sprites.Draw(_simpleHeart, Vector2.Zero, Vector2.Zero, Color.White);
            _sprites.Draw(_simpleHeart, Vector2.Zero, new Vector2(800 - 64, 480 - 64), Color.White);
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Draw(_sprites);
            }
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}