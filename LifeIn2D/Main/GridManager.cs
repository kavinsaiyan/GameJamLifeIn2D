using LifeIn2D.Entities;
using LifeIn2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
    }
}