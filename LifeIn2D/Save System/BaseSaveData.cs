using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public abstract class BaseSaveData
{
    public const string BASE_DIRECTORY = "Savedata/";
    protected abstract string FileName { get; }
    protected abstract object Data { get; set; }
    protected abstract Type Type { get; }

    public abstract void Init();

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream memoryStream = File.Open(GetFilePath(), FileMode.OpenOrCreate, FileAccess.Write))
            binaryFormatter.Serialize(memoryStream, Data);
    }

    private string GetFilePath()
    {
        return BASE_DIRECTORY + FileName;
    }

    protected void EnsureFileCreated()
    {
        if (Directory.Exists(BASE_DIRECTORY) == false)
            Directory.CreateDirectory(BASE_DIRECTORY);
        if (File.Exists(GetFilePath()) == false)
            File.Create(GetFilePath()).Dispose();
    }

    public void Load()
    {
        string json = File.ReadAllText(GetFilePath());
        if (string.IsNullOrEmpty(json) == false)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream memoryStream = File.OpenRead(GetFilePath()))
                Data = binaryFormatter.Deserialize(memoryStream);
        }
        else
            Save();
    }
}