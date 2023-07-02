using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Entities
{
    public enum MergeDirection { Left, Up, Right, Down }

    public class Tile
    {
        private Texture2D _graphic;
        private TileID _id;
        public TileID Id { get => _id; }
        private Vector2 _origin;
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
            if (_graphic != null)
                _origin = new Vector2(_graphic.Width / 2, _graphic.Height / 2);
            else
                _origin = Vector2.Zero;
        }

        public void Rotate()
        {
            for (int i = 0; i < _mergeDirections.Length; i++)
                _mergeDirections[i] = (MergeDirection)((int)_mergeDirections[i] + 1);
            _angle += MathHelper.PiOver2;
        }

        public void Draw(Sprites sprites)
        {
            sprites.Draw(_graphic, null, _origin, _position, _angle, Vector2.One, Color.White);
        }
    }
}