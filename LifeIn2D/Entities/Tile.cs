using System;
using LifeIn2D.Input;
using LifeIn2D.Main;
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
        private SimpleButton _button;

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
                _mergeDirections[i] = (MergeDirection)((int)_mergeDirections[i] + 1);
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
    }
}