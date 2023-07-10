using LifeIn2D.Entities;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

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
                // Logger.Log("current Tile is " + current.tile.Id + " index is row " + current.rowIndex + " col " + current.colIndex);
                if (current.tile.Id == TileID.Brain)
                {
                    Logger.Log("path is present to brain");
                    break;
                }

                List<TilePos> neighbourTiles = new List<TilePos>();
                void CheckAndAddNeighbourTile(int rowIndex, int colIndex, MergeDirection mergeDirection)
                {
                    if (rowIndex >= tileGrid.GetLength(0) || rowIndex < 0)
                    {
                        // Logger.Log("row index is out of grid length");
                        return;
                    }
                    if (colIndex >= tileGrid.GetLength(1) || colIndex < 0)
                    {
                        // Logger.Log("column index is out of grid length");
                        return;
                    }
                    Tile neighbourTile = tileGrid[rowIndex, colIndex];
                    // Logger.Log("row index and col index is present for " + neighbourTile.Id + " with r :" + rowIndex + ", c : " + colIndex);
                    if (neighbourTile.Id == TileID.None)
                    {
                        // Logger.Log("Tile id is None!");
                        return;
                    }
                    // Logger.Log("Neighbour tile id is " + neighbourTile.Id + " with mergerdirections " + string.Join(",", neighbourTile.MergeDirections));
                    if (current.tile.Contains(mergeDirection) == false)
                    {
                        // Logger.Log("Current tile does not contain direction " + mergeDirection);
                        return;
                    }
                    // Logger.Log("current tile also contains direction " + mergeDirection);
                    if (neighbourTile.ContainsEntryFor(mergeDirection) == false)
                    {
                        // Logger.Log("Neighour Tile {neighbourTile.ID} does not contain entry for direction {mergeDirection}");
                        return;
                    }
                    // Logger.Log("neighbour tile also contains entry direction " + mergeDirection);
                    if (neighbourTile.IsVisited == true)
                    {
                        // Logger.Log("Neighbour tile is already visited!");
                        return;
                    }
                    TilePos tilePos = new TilePos(rowIndex, colIndex, neighbourTile);
                    // Logger.Log("enqueing tile " + tilePos.tile.Id);
                    queue.Enqueue(tilePos);
                }
                CheckAndAddNeighbourTile(current.rowIndex + 1, current.colIndex, MergeDirection.Down);
                CheckAndAddNeighbourTile(current.rowIndex - 1, current.colIndex, MergeDirection.Up);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex + 1, MergeDirection.Right);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex - 1, MergeDirection.Left);
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