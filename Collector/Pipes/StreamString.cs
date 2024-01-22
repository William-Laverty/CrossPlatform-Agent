using System;
using System.IO;
using System.Text;
using Collector.Logs;

namespace Collector.Pipes
{
    /// <summary>
    /// Helper class for reading and writing strings to a stream.
    /// </summary>
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        /// <summary>
        /// Initializes a new instance of the StreamString class.
        /// </summary>
        /// <param name="ioStream">The underlying stream for reading and writing.</param>
        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        /// <summary>
        /// Reads a string from the stream.
        /// </summary>
        /// <returns>The string read from the stream.</returns>
        public string ReadString()
        {
            int len = 0;

            try
            {
                // Read the length of the incoming string
                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);

                // Convert the byte array to a string
                return streamEncoding.GetString(inBuffer);
            }
            catch (IOException e)
            {
                // Log any IOException that occurs during string reading
                Logger.Log($"Error reading string from stream: {e.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Writes a string to the stream.
        /// </summary>
        /// <param name="outString">The string to write to the stream.</param>
        /// <returns>The total number of bytes written, including the length prefix.</returns>
        public int WriteString(string outString)
        {
            try
            {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;

                // Ensure the length can be represented using two bytes
                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }

                // Write the length as two bytes
                ioStream.WriteByte((byte)(len / 256));
                ioStream.WriteByte((byte)(len & 255));

                // Write the string data
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 2;
            }
            catch (IOException e)
            {
                // Log any IOException that occurs during string writing
                Logger.Log($"Error writing string to stream: {e.Message}");
                return 0;
            }
        }
    }
}
