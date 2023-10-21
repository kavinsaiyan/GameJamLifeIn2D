using System;
using System.IO;
using System.Text.Json;

public abstract class BaseSaveData
{
    public const string BASE_DIRECTORY = "Savedata/";
    protected abstract string FileName { get; }
    protected abstract object Data { get; set; }
    protected abstract Type Type { get; }

    public void Save()
    {
        EnsureFileCreated();
        string json = JsonSerializer.Serialize(Data);
        File.WriteAllText(GetFilePath(), json);
    }

    private string GetFilePath()
    {
        return BASE_DIRECTORY + FileName;
    }

    private void EnsureFileCreated()
    {
        if(Directory.Exists(BASE_DIRECTORY) == false)
            Directory.CreateDirectory(BASE_DIRECTORY);
        if(File.Exists(GetFilePath()) == false)
            File.Create(GetFilePath());
    }

    public void Load()
    {
        EnsureFileCreated();
        string json = File.ReadAllText(GetFilePath());
        Data = JsonSerializer.Deserialize(json,Type);
    }
}