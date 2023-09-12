using System;

public interface ILogger
{
    public static ILogger Instance { get; }
    public void Log(string message);
    public void LogWarning(string message);
    public void LogError(string message);
    public void Clear();
}