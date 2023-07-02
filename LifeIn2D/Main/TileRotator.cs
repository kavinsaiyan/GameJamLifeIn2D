using LifeIn2D.Entities;

namespace LifeIn2D.Main
{
    public static class TileRotator
    {
        public static TileID GetNextRotation(TileID tileID)
        {
            //TODO: define rotations
            Logger.LogError("Tile rotation not defined for " + tileID);
            return TileID.None;
        }
    }
}