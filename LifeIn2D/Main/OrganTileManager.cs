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
            organTile = null;
            if (tile.Id == TileID.Dest_left || tile.Id == TileID.Dest_Down
                || tile.Id == TileID.Dest_right || tile.id == tileid.dest_up)
            {
                texture2d graphic = null;
                if (organGraphics.Count > 0)
                {
                    int randIndex= CommonFunctions.RandomRangeInt(0, organGrahpics.Count);
                    graphic = organGraphics[randIndex];
                    organGraphics.Remove(randIndex);
                }
                else
                    graphic = content.Load<Texture2D>("Muscle_Tile");
                organtile = new organtile(graphic, tile);
                return true;
            }
            return false;
        }

       public void Draw(Sprites sprites)
       {
            for(int i=0;i<organTiles.Count;i++)
                organTiles[i].Draw(sprites);
       } 
    }
}