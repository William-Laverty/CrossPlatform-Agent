using System;
using System.IO;

namespace Director.Logs
{
    /// <summary>
    /// Logger class for the Director.
    /// </summary>
    public static class Logger
    {
        private static readonly string logFilePath = "director_log.txt";

        /// <summary>
        /// Logs a message to the file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                    writer.WriteLine(logEntry);
                    Console.WriteLine(logEntry);  // Optionally, also log to the console
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
