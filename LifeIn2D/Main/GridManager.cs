using LifeIn2D.Entities;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
namespace LifeIn2D.Main
{
    public class GridManager
    {
        float rowCount;
        float rowWidth;
        float cornorPos;
        float xPos;
        float yPos;


        public int[,] grid = new int[5, 5]
        {
            {11, 0 , 5 , 14, 14},
            {14, 14, 2 , 14, 14},
            {14, 14, 3 , 5 , 14},
            {14, 14, 14, 1 , 14},
            {14, 14, 14, 3 , 12},
        };

        public Tile[,] tileGrid;

        public void Initialize(int width, int height, ContentManager contentManager)
        {
            rowCount = grid.GetLength(0);
            rowWidth = rowCount * 64;
            cornorPos = rowWidth / 2;
            xPos = -cornorPos + width / 2;
            yPos = cornorPos + height / 2;
            tileGrid = new Tile[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    // Logger.Log($"grid pos {i},{j} is {new Vector2(xPos, yPos)}");
                    tileGrid[i, j] = TileCreator.CreateTile(grid[i, j], new Vector2(xPos, yPos), contentManager);
                    xPos += 64;

                }
                xPos = -cornorPos + width / 2;
                yPos -= 64;
            }
        }


        public void Update(GameTime gameTime, Vector2 mousePos, bool isMouseClicked)
        {
            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Id != TileID.None)
                        tileGrid[i, j].Update(gameTime, mousePos, isMouseClicked);
                }
            }
        }

        public void Draw(Sprites sprites)
        {
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Id != TileID.None)
                        tileGrid[i, j].Draw(sprites);
                }
            }
        }

        public void FindPath()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Id != TileID.None)
                        tileGrid[i, j].IsVisited = false;
                }
            }
            //make a queue 
            Queue<TilePos> queue = new Queue<TilePos>();
            //add the heart to the queue
            queue.Enqueue(new TilePos(0, 0, tileGrid[0, 0]));
            while (queue.Count > 0)
            {
                TilePos current = queue.Dequeue();
                current.tile.IsVisited = true;
                if (current.tile.Id == TileID.Brain)
                {
                    Logger.Log("path is present to brain");
                }

                List<TilePos> neighbourTiles = new List<TilePos>();
                //right
                if (current.rowIndex + 1 < tileGrid.GetLength(0) && current.colIndex < tileGrid.GetLength(1))
                    neighbourTiles.Add(new TilePos(current.rowIndex + 1, current.colIndex, tileGrid[current.rowIndex + 1, current.colIndex]));
                //left
                if (current.rowIndex - 1 > 0 && current.colIndex < tileGrid.GetLength(1))
                    neighbourTiles.Add(new TilePos(current.rowIndex - 1, current.colIndex, tileGrid[current.rowIndex + 1, current.colIndex]));
                //bottom
                if (current.rowIndex < tileGrid.GetLength(0) && current.colIndex + 1 < tileGrid.GetLength(1))
                    neighbourTiles.Add(new TilePos(current.rowIndex, current.colIndex + 1, tileGrid[current.rowIndex, current.colIndex + 1]));
                //top
                if (current.rowIndex < tileGrid.GetLength(0) && current.colIndex - 1 > 0)
                    neighbourTiles.Add(new TilePos(current.rowIndex, current.colIndex - 1, tileGrid[current.rowIndex, current.colIndex - 1]));

                for (int i = 0; i < current.tile.MergeDirections.Length; i++)
                {
                    for (int j = 0; j < neighbourTiles.Count; j++)
                    {
                        if (neighbourTiles[j].tile.Id != TileID.None
                            && neighbourTiles[j].tile.Contains(current.tile.MergeDirections[i])
                            && neighbourTiles[j].tile.IsVisited == false)
                        {
                            queue.Enqueue(neighbourTiles[j]);
                        }
                    }
                }
            }

        }
    }

    public class TilePos
    {
        public int rowIndex;
        public int colIndex;
        public Tile tile;

        public TilePos(int rowIndex, int colIndex, Tile tile)
        {
            this.rowIndex = rowIndex;
            this.colIndex = colIndex;
            this.tile = tile;
        }
    }
}