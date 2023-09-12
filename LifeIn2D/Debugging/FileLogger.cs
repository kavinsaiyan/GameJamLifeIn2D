using System;
using System.IO;
using System.Net.Http;

namespace LifeIn2D
{
    public class FileLogger : ILogger
    {
        private const string PATH = @"Debugging/Logs.txt";
        private static FileLogger instance; 
        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                    instance = new FileLogger();
                return instance;
            }
        }
        public void Log(string message)
        {
            LogMessage(message, "INFO");
        }

        public void LogWarning(string message)
        {
            LogMessage(message, "WARNING");
        }

        public void LogError(string message)
        {
            LogMessage(message, "ERROR");
        }

        public void Clear()
        {
            File.WriteAllText(PATH,"");
        }

        public void LogMessage(string message, string messageType)
        {
            if (File.Exists(PATH))
            {
                using (StreamWriter fs = File.AppendText(PATH))
                {
                    fs.WriteLine(message);
                }
            }
        }
    }
}