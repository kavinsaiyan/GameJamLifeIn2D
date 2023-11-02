using System;
using System.Collections.Generic;
using System.Data.Common;
using LifeIn2D;
[Serializable]
public class LevelSaveData : BaseSaveData
{
    protected override string FileName => "LevelSaveData.dat";
    protected override Type Type => typeof(LevelSaveData);

    private static LevelSaveData _levelSaveData;
    public static LevelSaveData Instance
    {
        get
        {
            if (_levelSaveData == null)
                _levelSaveData = new LevelSaveData();
            return _levelSaveData;
        }
    }
    protected override object Data
    {
        get
        {
            return Instance;
        }
        set
        {
            _levelSaveData = (LevelSaveData)value;
        }
    }

    public int completedLevelCount = 0;
    public List<LevelSaveItem> levelSaveItems = new List<LevelSaveItem>();

    public override void Init()
    {
        EnsureFileCreated();
        Load();
    }

    public void InitializeSaveItems(int count)
    {
        bool addedNewSaveItem = false;
        for (int i = 0; i < count; i++)
        {
            if (levelSaveItems.Count < i + 1)
            {
                LevelSaveItem levelSaveItem = new LevelSaveItem() { levelNumber = i + 1, levelState = LevelState.Locked };
                // if (i == 0 || i == 14)
                if (i == 0 )
                    levelSaveItem.levelState = LevelState.Playable;
                levelSaveItems.Add(levelSaveItem);
                addedNewSaveItem = true;
            }
        }
        if (addedNewSaveItem)
            Save();
    }

    public bool TryGetSaveItem(int levelNumber, out LevelSaveItem levelSaveItem)
    {
        levelSaveItem = null;
        for (int i = 0; i < levelSaveItems.Count; i++)
        {
            if (levelSaveItems[i].levelNumber == levelNumber)
            {
                levelSaveItem = levelSaveItems[i];
                return true;
            }
        }
        return false;
    }
}

[Serializable]
public class LevelSaveItem
{
    public int levelNumber = 0;
    public LevelState levelState;
}