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
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_ThreeWay"), new MergeDirection[]
                    {
                        MergeDirection.Up,
                        MergeDirection.Down,
                        MergeDirection.Right,
                    }, 0, pos);
                case TileID.Threeway_rot90:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_ThreeWay"), new MergeDirection[]
                    {
                            MergeDirection.Up,
                            MergeDirection.Left,
                            MergeDirection.Right,
                    }, MathHelper.ToRadians(90), pos);
                case TileID.Threeway_rot180:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_ThreeWay"), new MergeDirection[]
                    {
                            MergeDirection.Down,
                            MergeDirection.Left,
                            MergeDirection.Up,
                    }, MathHelper.ToRadians(180), pos);
                case TileID.Threeway_rot270:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Tile_ThreeWay"), new MergeDirection[]
                    {
                            MergeDirection.Down,
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
                        MergeDirection.Left,
                        MergeDirection.Up,
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
                        MergeDirection.Right,
                        MergeDirection.Down,
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
                case TileID.Kidney:
                case TileID.Lungs:
                case TileID.Intestine:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("Player"),
                    new MergeDirection[]{
                        MergeDirection.Left,
                        MergeDirection.Right,
                        MergeDirection.Up,
                        MergeDirection.Down,
                    }, 0, pos);
                case TileID.Dest_Down:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("destination_bottom_entry"),
                    new MergeDirection[]{
                        MergeDirection.Down,
                    },0,pos);
                case TileID.Dest_Right:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("destination_right_entry"),
                    new MergeDirection[]{
                        MergeDirection.Right,
                    },MathHelper.ToRadians(90),pos);
                case TileID.Dest_Up:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("destination_top_entry"),
                    new MergeDirection[]{
                        MergeDirection.Up,
                    },MathHelper.ToRadians(180),pos);
                case TileID.Dest_Left:
                    return new Tile((TileID)id, contentManager.Load<Texture2D>("destination_left_entry"),
                    new MergeDirection[]{
                        MergeDirection.Left,
                    },MathHelper.ToRadians(270),pos);
                case TileID.None:
                    return new Tile(TileID.None, null, null, 0, pos);
            }
            Logger.LogError("[TileCreator.cs/CreateTile]: no graphic found for creating the tile id " + id);
            return new Tile(TileID.None, null, null, 0, pos);
        }
    }
}