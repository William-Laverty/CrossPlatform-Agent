using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Collector.Logs;

namespace Collector.Scripts
{
    /// <summary>
    /// Provides functionality for executing commands related to user sessions and extracting relevant information.
    /// </summary>
    public class UserSessions
    {
        /// <summary>
        /// Executes the specified command and extracts user session information from the output.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A formatted string containing user session information.</returns>
        public static string Execute(string command)
        {
            try
            {
                string output = ExecuteCommand(command);

                // Extract and format user login information using Regex
                string formattedOutput = ExtractFormattedUserSessions(output);

                Console.WriteLine("COLLECTOR: Fetched user session");

                return formattedOutput;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"COLLECTOR: Error executing command: {ex.Message}");
                return string.Empty;
            }
        }

        private static string ExecuteCommand(string command)
        {
            string output = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();

                    using (var sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine(command);
                        }
                    }

                    output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var proccessStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process {  StartInfo = proccessStartInfo })
                {
                    process.Start();

                    output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception($"Command execution error: {error}");
                    }
                }
            }
            return output;
        }

        private static string ExtractFormattedUserSessions(string input)
        {
            string formattedOutput = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string pattern = @"(\S+)\s+\S+\s+(\S+)\s+(\S+)\s+\S+\s+.*";
                MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Multiline);

                formattedOutput = "User Session Information:\n";

                foreach (Match match in matches)
                {
                    string username = match.Groups[1].Value;
                    string lastAction = match.Groups[2].Value.ToLower() == "still" ? "Login" : "Logout";
                    string timestamp = match.Groups[3].Value;

                    formattedOutput += $"USERNAME: {username}, LAST_ACTION: {lastAction}, TIMESTAMP: {timestamp}\n";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                formattedOutput = "User Session Information:\n";

                formattedOutput += $"{input}";
            }

            return formattedOutput;
        }
    }
}
