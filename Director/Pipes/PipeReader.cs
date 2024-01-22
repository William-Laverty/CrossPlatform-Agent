using System.IO.Pipes;
using System.Text;
using Director.Logs;

namespace Director.Pipes
{
    /// <summary>
    /// Provides functionality for reading data from a named pipe.
    /// </summary>
    public class PipeReader
    {
        /// <summary>
        /// Reads data from the specified named pipe client stream.
        /// </summary>
        /// <param name="pipeClient">The named pipe client stream to read data from.</param>
        /// <returns>A string containing the data read from the named pipe.</returns>
        public static string ReadDataFromPipe(NamedPipeClientStream pipeClient)
        {
            // StringBuilder to store the read data
            StringBuilder sb = new StringBuilder();

            try
            {
                // Use StreamReader to efficiently read from the named pipe client stream
                using (StreamReader reader = new StreamReader(pipeClient, Encoding.UTF8, true, 4096, true))
                {
                    // Read until the end of the stream and append each line to the StringBuilder
                    while (!reader.EndOfStream)
                    {
                        sb.Append(reader.ReadLine());
                    }
                }
            }
            catch (IOException ex)
            {
                // Log any exceptions that occur during reading
                Logger.Log($"DIRECTOR: Error reading data from pipe: {ex.Message}");
            }

            // Return the collected data as a string
            return sb.ToString();
        }
    }
}
