using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Collector
{
    public class UserSessions
    {
        public static string Execute(string command)
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

            var process = new Process { StartInfo = processStartInfo };

            process.Start();

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                }
            }

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Extract and format user login information using Regex
            string formattedOutput = ExtractFormattedUsernamesAndLastLogin(output);

            Console.WriteLine("COLLECTOR: Fetched user session");

            return formattedOutput;
        }

        private static string ExtractFormattedUsernamesAndLastLogin(string input)
        {
            // Define a Regex pattern for lines containing user login information
            string pattern = @"(\S+)\s+\S+\s+\S+\s+(\S+)\s+\S+\s+.+";

            // Use Regex to match lines, excluding the header line
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Multiline);

            // Construct a formatted output
            string formattedOutput = "User Login Information:\n";

            foreach (Match match in matches)
            {
                formattedOutput += $"USERNAME: {match.Groups[1].Value}, LAST_LOGGED_IN: {match.Groups[2].Value}\n";
            }

            return formattedOutput;
        }
    }
}
