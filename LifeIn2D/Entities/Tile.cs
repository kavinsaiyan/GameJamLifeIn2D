using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Entities
{
    public enum MergeDirection { None, Left, Right, Up, Down }

    public class Tile
    {
        private Texture2D _graphic;
        private MergeDirection[] _mergeDirections;
        private float _angle;
        private Vector2 _position;

        public Tile(Texture2D graphic, MergeDirection[] mergeDirections, float angle, Vector2 position)
        {
            _graphic = graphic;
            _mergeDirections = mergeDirections;
            _angle = angle;
            _position = position;
        }
    }
}