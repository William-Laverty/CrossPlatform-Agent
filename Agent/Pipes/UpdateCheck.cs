using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Director.Pipes
{
	public class UpdateCheck
	{
		public static string ExtractHasDataUpdated(string input)
		{
            MatchCollection matches = Regex.Matches(@input, @"\b(True|False)\b");
            string output = "";

            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
                output = match.Value;
            }

            Console.WriteLine($"BOOL: {output}");
			return output;
		}

    }
}

