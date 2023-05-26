using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeIn2D.Input;
namespace LifeIn2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Sprites _sprites;
        private Texture2D _simpleHeart;
        private Vector2 position;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprites = new Sprites(this);
            _simpleHeart = Content.Load<Texture2D>("SimpleHeart");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int x = 0, y = 0;
            CustomKeyboard.Instance.Update();
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.W) || CustomKeyboard.Instance.IsKeyClicked(Keys.Up)) { y++; }
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.S) || CustomKeyboard.Instance.IsKeyClicked(Keys.Down)) { y--; }
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.A) || CustomKeyboard.Instance.IsKeyClicked(Keys.Left)) { x++; }
            if (CustomKeyboard.Instance.IsKeyClicked(Keys.D) || CustomKeyboard.Instance.IsKeyClicked(Keys.Right)) { x--; }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _sprites.Begin(false);
            _sprites.Draw(_simpleHeart, Vector2.Zero, Vector2.Zero, Color.White);
            _sprites.Draw(_simpleHeart, Vector2.Zero, new Vector2(800 - 64, 480 - 64), Color.White);
            _sprites.DrawCircle(Vector2.One * 10, 4, 32, Color.White);
            _sprites.End();

            base.Draw(gameTime);
        }
    }
}
