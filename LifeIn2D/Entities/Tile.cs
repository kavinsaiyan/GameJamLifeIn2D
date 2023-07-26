using System;
using LifeIn2D.Input;
using LifeIn2D.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Entities
{
    public enum MergeDirection { Left, Down, Right, Up, }

    public class Tile
    {
        private Texture2D _graphic;
        private TileID _id;
        public TileID Id { get => _id; }

        private Vector2 _origin;
        public Vector2 Origin { get => _origin; }
        private MergeDirection[] _mergeDirections;
        public MergeDirection[] MergeDirections => _mergeDirections;
        private float _angle;
        private Vector2 _position;
        public Vector2 Position { get => _position; }
        private bool _isVisited;
        public bool IsVisited { get => _isVisited; set => _isVisited = value; }

        public int Width => _graphic.Width;
        public int Height => _graphic.Height;


        public Tile(TileID id, Texture2D graphic, MergeDirection[] mergeDirections, float angle, Vector2 position)
        {
            _graphic = graphic;
            _mergeDirections = mergeDirections;
            _angle = angle;
            _position = position;
            _id = id;
            if (_id != TileID.None)
            {
                _origin = new Vector2(_graphic.Width / 2, _graphic.Height / 2);
            }
            else
                _origin = Vector2.Zero;
        }

        public void Rotate()
        {
            // Logger.Log("Rotate Tile");
            for (int i = 0; i < _mergeDirections.Length; i++)
                _mergeDirections[i] = (MergeDirection)(((int)_mergeDirections[i] + 1) % 4);
            _angle += MathHelper.PiOver2;
            _id = TileRotator.GetNextRotation(_id);
        }

        public void Draw(Sprites sprites)
        {
            sprites.Draw(_graphic, null, _origin, _position, _angle, Vector2.One, Color.White);
        }

        public bool ContainsEntryFor(MergeDirection mergeDirection)
        {
            mergeDirection = GetOppositeDirectionFor(mergeDirection);
            // Logger.Log("opp direction " + mergeDirection);
            return Contains(mergeDirection);
        }
        public bool Contains(MergeDirection mergeDirection)
        {
            if (_mergeDirections == null)
            {
                Logger.LogError("[Tile.cs/Contains]: Merge directions array is null for " + Id);
                return false;
            }
            // Logger.Log("current tile " + Id + " with merge " + string.Join(",", _mergeDirections));
            for (int i = 0; i < _mergeDirections.Length; i++)
            {
                if (_mergeDirections[i] == mergeDirection)
                    return true;
            }
            return false;
        }
        public static MergeDirection GetOppositeDirectionFor(MergeDirection mergeDirection)
        {
            switch (mergeDirection)
            {
                case MergeDirection.Up:
                    return MergeDirection.Down;
                case MergeDirection.Down:
                    return MergeDirection.Up;
                case MergeDirection.Left:
                    return MergeDirection.Right;
                case MergeDirection.Right:
                    return MergeDirection.Left;
            }
            Logger.LogError("Opposite not defined for " + mergeDirection);
            return MergeDirection.Up;
        }
    }
}