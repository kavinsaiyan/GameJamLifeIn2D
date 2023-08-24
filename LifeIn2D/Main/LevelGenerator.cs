using LifeIn2D.Entities;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace LifeIn2D.Main
{
    public class LevelGenerator
    {
        float rowCount;
        float rowWidth;
        float cornorPos;
        float xPos;
        float yPos;
        List<TileID> _destinations = new List<TileID>();

        public event System.Action<Tile> OnTileCreated;
        public event System.Action OnPathFound;


        public int[,] grid = new int[5, 5]
        {
            {12,4,-1,3,14},
            {-1,1,-1,1,-1},
            {-1,7,11,8,-1},
            {-1,0,-1,0,-1},
            {15,5,-1,2,13},
        };

        public Tile[,] tileGrid;

        public void Initialize(int width, int height, ContentManager contentManager, IEnumerable<TileID> destinations)
        {
            rowCount = grid.GetLength(0);
            rowWidth = rowCount * 64;
            cornorPos = rowWidth / 2;
            xPos = -cornorPos + width / 2;
            yPos = cornorPos + height / 2;
            _destinations.Clear();
            _destinations.AddRange(destinations);
            tileGrid = new Tile[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    // Logger.Log($"grid pos {i},{j} is {new Vector2(xPos, yPos)}");
                    tileGrid[i, j] = TileCreator.CreateTile(grid[i, j], new Vector2(xPos, yPos), contentManager);
                    xPos += 64;
                    OnTileCreated?.Invoke(tileGrid[i, j]);
                }
                xPos = -cornorPos + width / 2;
                yPos -= 64;
            }
        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(Sprites sprites)
        {
            if (grid == null)
                return;
        }

        public void FindPath()
        {
            System.Console.Clear();
            //make a queue 
            Queue<TilePos> queue = new Queue<TilePos>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (tileGrid[i, j].Id == TileID.None)
                        tileGrid[i, j].IsVisited = true;
                    else
                        tileGrid[i, j].IsVisited = false;

                    if (tileGrid[i, j].Id == TileID.Brain)
                        queue.Enqueue(new TilePos(i, j, tileGrid[i, j]));
                }
            }
            // Logger.LogWarning("queue count" + queue.Count);
            while (queue.Count > 0)
            {
                TilePos current = queue.Dequeue();
                current.tile.IsVisited = true;
                // Logger.Log("current Tile is " + current.tile.Id + " index is row " + current.rowIndex + " col " + current.colIndex);
                if (current.tile.Id == TileID.Heart)
                {
                    // Logger.Log("path is present to Heart and is as follows");
                    TilePos temp = current;
                    while(temp != null )
                    {
                        // Logger.Log($"\tTileid:{temp.tile.Id} rowIndex:{temp.rowIndex} colIndex:{temp.colIndex}");
                        temp = temp.parent;
                    }
                    OnPathFound?.Invoke();
                    break;
                }

                // Logger.Log(" current tile is " + current.tile.Id + " at pos " + current.rowIndex + " , " + current.colIndex);
                void CheckAndAddNeighbourTile(int rowIndex, int colIndex )
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
                    // Logger.Log(" row index and col index is present for " + neighbourTile.Id + " with r :" + rowIndex + ", c : " + colIndex);
                    if (neighbourTile.IsVisited == true)
                    {
                        // Logger.Log("  Neighbour tile is already visited!");
                        return;
                    }
                    if (neighbourTile.Id == TileID.None)
                    {
                        // Logger.Log("  Tile id is None!");
                        return;
                    }
                    TilePos tilePos = new TilePos(rowIndex, colIndex, neighbourTile)
                    {
                        parent = current
                    };
                    // Logger.Log("    enqueing tile " + tilePos.tile.Id);
                    queue.Enqueue(tilePos);
                }
                CheckAndAddNeighbourTile(current.rowIndex + 1, current.colIndex);
                CheckAndAddNeighbourTile(current.rowIndex - 1, current.colIndex);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex + 1);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex - 1);
            }
        }
        public void Reset()
        {
            tileGrid = null;
            grid = null;
        }
    }
}