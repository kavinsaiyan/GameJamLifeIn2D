using System.IO;
using LifeIn2D.Entities;
using Microsoft.Xna.Framework;

namespace LifeIn2D
{
    public enum LevelLoadingState { None, Header, CurrentLevel, Destinations, Row, Col, LevelContent, Done, }
    public class LevelLoader
    {
        public const string FILE_PATH = "Text files/Levels.txt";
        public int[,] grid;
        public int currentLevel;
        int rows, columns;
        public TileID[] destinations;
        public LevelLoadingState state = LevelLoadingState.None;
        public LevelLoader()
        {
            currentLevel = 1;
        }


        public void Load()
        {
            using (StreamReader reader = new StreamReader(FILE_PATH))
            {
                string line;
                int levelContentReadCount = 0;

                // Read each line until the end of the file is reached
                while ((line = reader.ReadLine()) != null)
                {
                    // Logger.Log("level laoder state " + state);

                    // Process the line here (e.g., print it)
                    if (line.Equals("#####"))
                    {
                        state = LevelLoadingState.Header;
                        continue;
                    }
                    if (state == LevelLoadingState.Header
                        && int.TryParse(line, out int currentLevelNumber)
                        && currentLevel == currentLevelNumber)
                    {
                        state = LevelLoadingState.Destinations;
                        continue;
                    }
                    if(state == LevelLoadingState.Destinations)
                    {
                        string[] split = line.Split(",");
                        destinations = new TileID[split.Length];
                        for (int i = 0; i < destinations.Length; i++)
                        {
                            if(int.TryParse(split[i],out int destination))
                                destinations[i] = (TileID)destination; 
                        }
                        state = LevelLoadingState.Row;
                        continue;
                    }
                    if (state == LevelLoadingState.Row && int.TryParse(line, out int row))
                    {
                        state = LevelLoadingState.Col;
                        rows = row;
                        continue;
                    }
                    if (state == LevelLoadingState.Col && int.TryParse(line, out int col))
                    {
                        state = LevelLoadingState.LevelContent;
                        columns = col;
                        grid = new int[rows, columns];
                        levelContentReadCount = 0;
                        continue;
                    }
                    if (state == LevelLoadingState.LevelContent && levelContentReadCount < rows)
                    {
                        string[] split = line.Split(",", columns);
                        for (int i = 0; i < columns; i++)
                        {
                            if (int.TryParse(split[i], out int tileValue))
                            {
                                grid[levelContentReadCount, i] = tileValue;
                            }
                        }
                        levelContentReadCount++;
                        if (levelContentReadCount == rows)
                        {
                            // Logger.Log("level loader loaded level " + currentLevel);
                            state = LevelLoadingState.Done;
                            break;
                        }
                        continue;
                    }
                    state = LevelLoadingState.None;
                }
            }
        }
    }
}