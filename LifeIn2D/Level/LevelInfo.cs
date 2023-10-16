using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LifeIn2D
{
    public class LevelInfo
    {
        private List<LevelData> _levelDatas;
        public IReadOnlyList<LevelData> LevelDatas => _levelDatas;

        private LevelLoader _levelLoader;

        public LevelInfo()
        {
            _levelLoader = new LevelLoader();
            _levelLoader.currentLevel = 1;
            _levelLoader.LoadLevelCount();
            _levelDatas = new List<LevelData>(_levelLoader.levelsCount);
            for (int i = 0; i < _levelLoader.levelsCount; i++)
            {
                _levelLoader.LoadLevel();
                LevelData levelData = new LevelData()
                {
                    levelNumber = _levelLoader.currentLevel,
                    rows = _levelLoader.rows,
                    columns = _levelLoader.columns,
                    destinationsCount = _levelLoader.destinationsCount,
                    grid = new int[_levelLoader.rows, _levelLoader.columns],
                };
                Array.Copy(_levelLoader.grid, levelData.grid, levelData.rows * levelData.columns);
                _levelDatas.Add(levelData);
                _levelLoader.currentLevel += 1;
            }
        }
    }
}