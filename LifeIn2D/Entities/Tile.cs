using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Entities
{
    public enum MergeDirection { None, Left, Right, Up, Down }

    public class Tile
    {
        private Texture2D _graphic;
        private TileID _id;
        private MergeDirection[] _mergeDirections;
        private float _angle;
        private Vector2 _position;

        public Tile(TileID id, Texture2D graphic, MergeDirection[] mergeDirections, float angle, Vector2 position)
        {
            _graphic = graphic;
            _mergeDirections = mergeDirections;
            _angle = angle;
            _position = position;
            _id = id;
        }

        public TileID Id { get => _id; set => _id = value; }

        public void Draw(Sprites sprites)
        {
            sprites.Draw(_graphic, null, new Vector2(_graphic.Width / 2, _graphic.Height / 2), _position, _angle, Vector2.One, Color.White);
        }
    }
}