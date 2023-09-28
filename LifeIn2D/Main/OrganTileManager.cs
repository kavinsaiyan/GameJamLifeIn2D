using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CodingMath.Utils;
using System.Runtime.CompilerServices;
using LifeIn2D.Entities;

namespace LifeIn2D.Main
{
    public class OrganTileManager
    {
        public List<string> organGraphics = new List<string>();
        private List<OrganTile> organTiles = new List<OrganTile>();
        public void Initialize()
        {
            organGraphics = new List<string>() { "Eye_Tile", "Brain_Tile", "Stomach_Tile", "Liver_Tile", "Kidney_Tile" };
            organTiles.Clear();
        }

        public bool CreateOrganIfPossible(Tile tile, ContentManager content, out OrganTile organTile)
        {
            // Logger.Instance.Log("tile is " + tile.Id);
            organTile = null;
            if (tile.Id == TileID.Dest_Left || tile.Id == TileID.Dest_Down
                || tile.Id == TileID.Dest_Right || tile.Id == TileID.Dest_Up)
            {
                Texture2D graphic;
                // Logger.Instance.Log("tile is 1 " + tile.Id);
                if (organGraphics.Count > 0)
                {
                    int randIndex = CommonFunctions.RandomRangeInt(0, organGraphics.Count);
                    graphic = content.Load<Texture2D>(organGraphics[randIndex]);
                    // Logger.Instance.Log("removeing " + organGraphics[randIndex]);
                    organGraphics.RemoveAt(randIndex);
                }
                else
                    graphic = content.Load<Texture2D>("Muscle_tile");
                organTile = new OrganTile(graphic, tile);
                organTiles.Add(organTile);
                return true;
            }
            return false;
        }

        public void Draw(Sprites sprites)
        {
            for (int i = 0; i < organTiles.Count; i++)
                organTiles[i].Draw(sprites);
        }
    }
}