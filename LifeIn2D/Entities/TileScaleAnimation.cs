using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace LifeIn2D.Entities
{
    public class TileScaleAnimation
    {
        private Tile _tile;
        public TileScaleAnimation(Tile tile)
        {
            _tile = tile;
        }
        public void Update(GameTime gameTime)
        {
            Vector2 scale = _tile.Scale;

            double sin = Math.Abs(Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * Math.PI * 0.1f)*0.1f)+0.9f;
            scale.X = scale.Y = (float)sin; 
            _tile.Scale = scale;
        }
    }
}
