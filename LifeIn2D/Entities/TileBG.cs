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
                case TileID.Brain:
                case TileID.Kidney:
                case TileID.Liver:
                case TileID.Eye:
                case TileID.Muscle:
                    _background = contentManager.Load<Texture2D>("destination_left_entry_blood");
                    break;
                case TileID.L_normal:
                    _background = contentManager.Load<Texture2D>("Tile_L_Blood");
                    break;
                case TileID.Threeway_normal:
                    _background = contentManager.Load<Texture2D>("Tile_Threeway_Blood");
                    break;
                case TileID.Vertical:
                    _background = contentManager.Load<Texture2D>("Tile_Vertical_Blood");
                    break;
                case TileID.Plus:
                    _background = contentManager.Load<Texture2D>("Tile_Plus_Blood");
                    break;
                case TileID.Heart:
                    _background = contentManager.Load<Texture2D>("HeartBG_Blood");
                    break;
            }
            Tile.OnRotateAnimationBegin += TurnOn;
            Tile.OnRotateAnimationComplete+= TurnOff;
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
                if (Tile.IsConnected == true)
                    sprites.Draw(_background, null, Tile.Origin / 2, Tile.Position, 
                     Tile.TargetRotatin, Vector2.One,Color.White);
            }
        }
    }
}