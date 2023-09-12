using LifeIn2D.Entities;

namespace LifeIn2D.Main
{
    public static class TileRotator
    {
        public static TileID GetNextRotation(TileID tileID)
        {
            switch (tileID)
            {
                case TileID.Horizontal:
                    return TileID.Vertical;
                case TileID.Vertical:
                    return TileID.Horizontal;
                case TileID.Plus:
                    return TileID.Plus;
                case TileID.L_normal:
                    return TileID.L_rot90;
                case TileID.L_rot90:
                    return TileID.L_rot180;
                case TileID.L_rot180:
                    return TileID.L_rot270;
                case TileID.L_rot270:
                    return TileID.L_normal;
                case TileID.Threeway_normal:
                    return TileID.Threeway_rot90;
                case TileID.Threeway_rot90:
                    return TileID.Threeway_rot180;
                case TileID.Threeway_rot180:
                    return TileID.Threeway_rot270;
                case TileID.Threeway_rot270:
                    return TileID.Threeway_normal;
                case TileID.Dest_Left:
                    return TileID.Dest_Down;
                case TileID.Dest_Down:
                    return TileID.Dest_Right;
                case TileID.Dest_Right:
                    return TileID.Dest_Up; 
                case TileID.Dest_Up:
                    return TileID.Dest_Left;
            }
            Logger.Instance.LogError("Tile rotation not defined for " + tileID);
            return TileID.None;
        }
    }
}