using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using Director.Logs;

namespace Director.Pipes
{
    /// <summary>
    /// Provides functionality for extracting information related to data updates.
    /// </summary>
    public class UpdateCheck
    {
        /// <summary>
        /// Extracts a boolean value indicating whether data has been updated from the given input string.
        /// </summary>
        /// <param name="input">The input string to search for boolean values.</param>
        /// <returns>A boolean value indicating whether data has been updated (True) or not (False).</returns>
        public static string ExtractHasDataUpdated(string input)
        {
            try
            {
                // Use a regular expression to find True/False in the input string
                MatchCollection matches = Regex.Matches(input, @"\b(True|False)\b");

                string output = "";

                // Iterate through matches and print each value
                foreach (Match match in matches)
                {
                    Logger.Log($"DIRECTOR: Extracted boolean value: {match.Value}");
                    output = match.Value;
                }

                Logger.Log($"DIRECTOR: BOOL: {output}");

                // Return the extracted boolean value
                return output;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during extraction
                Logger.Log($"DIRECTOR: Error extracting boolean value: {ex.Message}");
                return "";
            }
        }
    }
}
