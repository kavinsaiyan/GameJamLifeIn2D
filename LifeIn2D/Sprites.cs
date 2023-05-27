using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
namespace LifeIn2D
{
    public sealed class Sprites : IDisposable
    {
        private bool _isDisposed;
        private Game _game;
        private SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch => _spriteBatch;
        private BasicEffect _effect;
        public Sprites(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }
            _game = game;
            _isDisposed = false;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _effect = new BasicEffect(_game.GraphicsDevice);
            _effect.FogEnabled = false;
            _effect.TextureEnabled = true;
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.World = Matrix.Identity;
            _effect.Projection = Matrix.Identity;
            _effect.View = Matrix.Identity;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _spriteBatch.Dispose();
            _isDisposed = true;
        }

        public void Begin(bool isTextureFilteringEnabled)
        {
            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFilteringEnabled)
            {
                sampler = SamplerState.LinearClamp;
            }

            Matrix proj = Matrix.CreateOrthographicOffCenter(0, _game.GraphicsDevice.Viewport.Width, 0, _game.GraphicsDevice.Viewport.Height, 0, 1);
            _effect.Projection = proj;
            _effect.View = Matrix.Identity;

            _spriteBatch.Begin(SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: sampler, null, rasterizerState: RasterizerState.CullNone, effect: _effect);
        }

        public void End()
        {
            _spriteBatch.End();
        }

        public void Draw(Texture2D texture2D, Vector2 origin, Vector2 position, Color color)
        {
            _spriteBatch.Draw(texture2D, position, null, color, 0, origin, 1, SpriteEffects.FlipVertically, 0);
        }

        public void Draw(Texture2D texture2D, Rectangle? sourceRectangle, Vector2 origin, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            _spriteBatch.Draw(texture2D, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.FlipVertically, 0);
        }

        public void Draw(Texture2D texture2D, Rectangle? sourceRectangle, Rectangle destinationRectangle, Color color)
        {
            _spriteBatch.Draw(texture2D, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
        }
        public void DrawCircle(Vector2 position, float radius, int sides, Color color)
        {
            _spriteBatch.DrawCircle(position, radius, sides, color, 1, 0);
        }
        public void DrawRectangle(Vector2 position, float width, float height, Color color)
        {
            _spriteBatch.DrawRectangle(position.X, position.Y, width, height, color, 1, 0);
        }
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}