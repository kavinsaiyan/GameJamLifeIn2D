using System;
using Microsoft.Xna.Framework;

namespace LifeIn2D
{
    public class LevelData
    {
        public int levelNumber;
        public int rows;
        public int columns;
        public int[,] grid;
        public int destinationsCount;
    }
}