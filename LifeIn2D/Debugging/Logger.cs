using System;
namespace LifeIn2D
{
    public class Logger
    {
        public static void Log(string message)
        {
            LogMessage(message, ConsoleColor.White, "INFO");
        }

        public static void LogWarning(string message)
        {
            LogMessage(message, ConsoleColor.Yellow, "WARNING");
        }

        public static void LogError(string message)
        {
            LogMessage(message, ConsoleColor.Red, "ERROR");
        }

        private static void LogMessage(string message, ConsoleColor color, string messageType)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now}] [{messageType}] {message}");
            Console.ResetColor();
        }
    }
}