using System;
using System.IO;

namespace Collector.Logs
{
    /// <summary>
    /// Logger class for handling application logs.
    /// </summary>
    public class Logger
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// Writes a log message to the specified log file.
        /// </summary>
        /// <param name="message">The log message to be written.</param>
        public static void Log(string message)
        {
            string logFilePath = GetLogFilePath();

            lock (lockObject)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                        writer.WriteLine(logEntry);
                        Console.WriteLine(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }

        private static string GetLogFilePath()
        {
            // Get the directory of the current assembly
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "Unknown Path: Executing Assembly";

            // Combine the current directory with the log file path
            return Path.Combine(currentDirectory, "CollectorLog.txt");
        }
    }
}
