using System;
namespace LifeIn2D
{
    public class Logger :ILogger
    {
        private static Logger instance;
        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();
                return instance;
            }
        }
        public void Log(string message)
        {
            LogMessage(message, ConsoleColor.White, "INFO");
        }

        public void LogWarning(string message)
        {
            LogMessage(message, ConsoleColor.Yellow, "WARNING");
        }

        public void LogError(string message)
        {
            LogMessage(message, ConsoleColor.Red, "ERROR");
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void LogMessage(string message, ConsoleColor color, string messageType)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now}] [{messageType}] {message}");
            Console.ResetColor();
        }
    }
}