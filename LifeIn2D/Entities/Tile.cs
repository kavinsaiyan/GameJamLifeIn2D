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
        private MergeDirection[] _mergeDirections;
        public MergeDirection[] MergeDirections => _mergeDirections;
        private float _angle;
        private Vector2 _position;
        private SimpleButton _button;
        private bool _isVisited;
        public bool IsVisited { get => _isVisited; set => _isVisited = value; }

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
                _button = new SimpleButton(graphic.Width, graphic.Height, position - _origin);
                _button.OnClick += Rotate;
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

        public void Update(GameTime gameTime, Vector2 mousePos, bool isMouseClicked)
        {
            _button.Update(mousePos, isMouseClicked);
        }

        public void Draw(Sprites sprites)
        {
            sprites.Draw(_graphic, null, _origin, _position, _angle, Vector2.One, Color.White);
            _button.Draw(sprites);
        }

        public bool ContainsEntryFor(MergeDirection mergeDirection)
        {
            mergeDirection = GetOppositeDirectionFor(mergeDirection);
            // Logger.Log("opp direction " + mergeDirection);
            return Contains(mergeDirection);
        }
        public bool Contains(MergeDirection mergeDirection)
        {
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