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
        List<TileID> _destinations = new List<TileID>();

        public event System.Action<Tile> OnTileCreated;
        public event System.Action OnPathFound;


        public int[,] grid = new int[5, 5]
        {
            {11, 0 , 5 , 14, 14},
            {14, 14, 2 , 14, 14},
            {14, 14, 3 , 5 , 14},
            {14, 14, 14, 1 , 14},
            {14, 14, 14, 3 , 12},
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
            if(grid == null)
                return;
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
                    if (tileGrid[i, j].Id == TileID.Heart)
                        //add the heart to the queue
                        queue.Enqueue(new TilePos(i, j, tileGrid[i, j]));
                }
            }
            Logger.LogWarning("queue count" + queue.Count+"/m The destnations are "+string.Join(",",_destinations.Select(d => d.ToString())));
            List<TileID> tempDestinations = new List<TileID>(){TileID.Dest_Down,TileID.Dest_Left,TileID.Dest_Right,TileID.Dest_Up};
            int destinationsCount = _destinations.Count;
            while (queue.Count > 0)
            {
                TilePos current = queue.Dequeue();
                current.tile.IsVisited = true;
                Logger.Log("current Tile is " + current.tile.Id + " index is row " + current.rowIndex + " col " + current.colIndex);
                if (tempDestinations.Contains(current.tile.Id))
                {
                    destinationsCount--;
                    Logger.LogError("removinf tile : "+current.tile.Id);
                    if(destinationsCount == 0)
                    {
                        Logger.Log("    path is present to brain");
                        OnPathFound?.Invoke();
                        break;
                    }
                }

                Logger.Log(" current tile is " + current.tile.Id + " at pos " + current.rowIndex + " , " + current.colIndex);
                void CheckAndAddNeighbourTile(int rowIndex, int colIndex, MergeDirection mergeDirection)
                {
                    if (rowIndex >= tileGrid.GetLength(0) || rowIndex < 0)
                    {
                        Logger.Log("row index is out of grid length");
                        return;
                    }
                    if (colIndex >= tileGrid.GetLength(1) || colIndex < 0)
                    {
                        Logger.Log("column index is out of grid length");
                        return;
                    }
                    Tile neighbourTile = tileGrid[rowIndex, colIndex];
                    Logger.Log(" row index and col index is present for " + neighbourTile.Id + " with r :" + rowIndex + ", c : " + colIndex);
                    if (neighbourTile.IsVisited == true)
                    {
                        Logger.Log("  Neighbour tile is already visited!");
                        return;
                    }
                    if (neighbourTile.Id == TileID.None)
                    {
                        Logger.Log("  Tile id is None!");
                        return;
                    }
                    Logger.Log(" Neighbour tile id is " + neighbourTile.Id + " with mergerdirections " + string.Join(",", neighbourTile.MergeDirections));
                    if (current.tile.Contains(mergeDirection) == false)
                    {
                        Logger.Log("  Current tile does not contain direction " + mergeDirection);
                        return;
                    }
                    Logger.Log(" current tile also contains direction " + mergeDirection);
                    if (neighbourTile.ContainsEntryFor(mergeDirection) == false)
                    {
                        Logger.Log("  Neighour Tile {neighbourTile.ID} does not contain entry for direction {mergeDirection}");
                        return;
                    }
                    Logger.Log(" neighbour tile also contains entry direction " + mergeDirection);
                    TilePos tilePos = new TilePos(rowIndex, colIndex, neighbourTile);
                    Logger.Log("    enqueing tile " + tilePos.tile.Id);
                    queue.Enqueue(tilePos);
                }
                CheckAndAddNeighbourTile(current.rowIndex + 1, current.colIndex, MergeDirection.Down);
                CheckAndAddNeighbourTile(current.rowIndex - 1, current.colIndex, MergeDirection.Up);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex + 1, MergeDirection.Right);
                CheckAndAddNeighbourTile(current.rowIndex, current.colIndex - 1, MergeDirection.Left);
            }
        }
        public void Reset()
        {
            tileGrid = null;
            grid = null;
        }
    }


    public class TilePos
    {
        public int rowIndex;
        public int colIndex;
        public Tile tile;
        public TilePos parent = null;

        public TilePos(int rowIndex, int colIndex, Tile tile)
        {
            this.rowIndex = rowIndex;
            this.colIndex = colIndex;
            this.tile = tile;
        }
    }
}