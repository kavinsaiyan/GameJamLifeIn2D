using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace LifeIn2D.Entities
{
    public class TileBG
    {
        public Tile Tile { get; private set; }
        private Texture2D _background;
        private bool _canDraw = false;
        public TileBG(Tile tile, ContentManager contentManager)
        {
            Tile = tile;
            switch (tile.Id)
            {
                case TileID.Dest_Down:
                case TileID.Dest_Up:
                case TileID.Dest_Right:
                case TileID.Dest_Left:
                    _background = contentManager.Load<Texture2D>("destination_left_entry_Blood");
                    break;
                case TileID.L_normal:
                case TileID.L_rot180:
                case TileID.L_rot90:
                case TileID.L_rot270:
                    _background = contentManager.Load<Texture2D>("Tile_L_Blood");
                    break;
                case TileID.Threeway_normal:
                case TileID.Threeway_rot180:
                case TileID.Threeway_rot270:
                case TileID.Threeway_rot90:
                    _background = contentManager.Load<Texture2D>("Tile_ThreeWay_Blood");
                    break;
                case TileID.Vertical:
                case TileID.Horizontal:
                    _background = contentManager.Load<Texture2D>("Tile_Vertical_Blood");
                    break;
                case TileID.Plus:
                    _background = contentManager.Load<Texture2D>("Tile_Plus_Blood");
                    break;
                case TileID.Heart:
                    _background = contentManager.Load<Texture2D>("HeartBG_Blood");
                    break;
            }
            Tile.OnRotateAnimationBegin += TurnOff;
            Tile.OnRotateAnimationComplete += TurnOn;
            TurnOn();
        }

        public void TurnOff()
        {
            _canDraw = false;
        }
        public void TurnOn()
        {
            _canDraw = true;
        }
        public void Draw(Sprites sprites)
        {
            if (_canDraw)
            {
                if (Tile.IsVisited == true && _background != null)
                    sprites.Draw(_background, null, Tile.Origin , Tile.Position, 
                                    Tile.TargetRotatin, Vector2.One,Color.White);
            }
        }
    }
}