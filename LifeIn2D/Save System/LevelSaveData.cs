using System;
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
    public override void Init()
    {
        EnsureFileCreated();
        Load();
    }
}