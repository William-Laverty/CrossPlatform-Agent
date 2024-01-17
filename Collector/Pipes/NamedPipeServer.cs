using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace Collector
{
    public class NamedPipeServer
    {
        private static int numThreads = 1;

        public static void Start()
        {
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private static void ServerThread()
        {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("AgentPipe", PipeDirection.InOut, numThreads);

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("COLLECTOR: Director connected.");
            Console.WriteLine("Username: {0}", Environment.UserName);
            bool dataHasUpdated = true;

            try
            {
                StreamString ss = new StreamString(pipeServer);

                string userSessionsOutput = UserSessions.Execute("w");
                ss.WriteString(userSessionsOutput);

                string dataUpdate = dataHasUpdated.ToString();
                ss.WriteString(dataUpdate);
                dataHasUpdated = false;
            }
            catch (IOException e)
            {
                Console.WriteLine("COLLECTOR: ERROR: {0}", e.Message);
            }
            finally
            {
                pipeServer.Close();
            }
        }
    }

    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}