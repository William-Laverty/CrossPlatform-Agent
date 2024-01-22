using System.IO.Pipes;
using System.Text;
using Collector.Logs;

namespace Collector.Pipes
{
    /// <summary>
    /// Provides functionality for sending output through a named pipe.
    /// </summary>
    public class PipeSender
    {
        /// <summary>
        /// Sends the specified output through the given named pipe client stream.
        /// </summary>
        /// <param name="pipeClient">The named pipe client stream for sending output.</param>
        /// <param name="output">The output string to be sent through the named pipe.</param>
        public static void SendOutput(NamedPipeClientStream pipeClient, string output)
        {
            try
            {
                // Convert the output string to bytes using UTF-8 encoding
                byte[] buffer = Encoding.UTF8.GetBytes(output);

                // Write the buffer to the named pipe
                pipeClient.Write(buffer, 0, buffer.Length);

                // Log the successful sending of output
                Logger.Log($"Output sent through named pipe: {output}");
            }
            catch (IOException e)
            {
                // Log any IOException that occurs during output sending
                Logger.Log($"Error sending output through named pipe: {e.Message}");
            }
        }
    }
}
