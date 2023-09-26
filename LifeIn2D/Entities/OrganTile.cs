using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Versioning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Entities
{
    public class OrganTile
    {
        public Texture2D _organGraphic;
        public Tile _tile;

        public OrganTile(Texture2D organGraphic, Tile tile)
        {
            _organGraphic = organGraphic;
            _tile = tile;

        }

        public void Draw(Sprites sprites)
        {
            sprites.Draw(_graphic, null, new Vector2(0,0), _tile.Position, 0, Vector2.One, Color.White);
        }
    }
}
