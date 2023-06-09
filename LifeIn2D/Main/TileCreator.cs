

using LifeIn2D.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LifeIn2D.Main
{
    public static class TileCreator
    {
        public static Tile CreateTile(int id, Vector2 pos, ContentManager contentManager)
        {
            switch ((TileID)id)
            {
                case TileID.Horizontal:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Vertical"), new MergeDirection[]
                    {
                        MergeDirection.Left,
                        MergeDirection.Right,
                    }, MathHelper.ToRadians(90), pos);
                case TileID.Vertical:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Vertical"), new MergeDirection[]
                    {
                        MergeDirection.Up,
                        MergeDirection.Down,
                    }, 0, pos);
                case TileID.Plus:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Plus"), new MergeDirection[]
                    {
                        MergeDirection.Up,
                        MergeDirection.Down,
                        MergeDirection.Left,
                        MergeDirection.Right,
                    }, 0, pos);
                case TileID.Threeway_normal:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Threeway"), new MergeDirection[]
                    {
                        MergeDirection.Up,
                        MergeDirection.Down,
                        MergeDirection.Right,
                    }, 0, pos);
                case TileID.Threeway_rot180:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Threeway"), new MergeDirection[]
                    {
                            MergeDirection.Down,
                            MergeDirection.Left,
                            MergeDirection.Up,
                    }, MathHelper.ToRadians(180), pos);
                case TileID.Threeway_rot90:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Threeway"), new MergeDirection[]
                    {
                            MergeDirection.Down,
                            MergeDirection.Left,
                            MergeDirection.Right,
                    }, MathHelper.ToRadians(90), pos);
                case TileID.Threeway_rot270:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_Threeway"), new MergeDirection[]
                    {
                            MergeDirection.Up,
                            MergeDirection.Left,
                            MergeDirection.Right,
                    }, MathHelper.ToRadians(270), pos);
                case TileID.L_normal:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_L_Shape"),
                    new MergeDirection[]{
                        MergeDirection.Up,
                        MergeDirection.Right,
                    }, 0, pos);
                case TileID.L_rot90:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_L_Shape"),
                    new MergeDirection[]{
                        MergeDirection.Down,
                        MergeDirection.Right,
                    }, MathHelper.ToRadians(90), pos);
                case TileID.L_rot180:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_L_Shape"),
                    new MergeDirection[]{
                        MergeDirection.Down,
                        MergeDirection.Left,
                    }, MathHelper.ToRadians(180), pos);
                case TileID.L_rot270:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_L_Shape"),
                    new MergeDirection[]{
                        MergeDirection.Left,
                        MergeDirection.Up,
                    }, MathHelper.ToRadians(270), pos);
                case TileID.Heart:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("SimpleHeart"),
                    new MergeDirection[]{
                        MergeDirection.Left,
                        MergeDirection.Right,
                        MergeDirection.Up,
                        MergeDirection.Down,
                    }, 0, pos);
                case TileID.Brain:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Player"),
                    new MergeDirection[]{
                        MergeDirection.Left,
                        MergeDirection.Right,
                        MergeDirection.Up,
                        MergeDirection.Down,
                    }, 0, pos);
                case TileID.None:
                    return new Tile(TileID.None, null, null, 0, pos);
            }
            Logger.LogError("no graphic found for creating the tile");
            return new Tile(TileID.None, null, null, 0, pos);
        }
    }
}