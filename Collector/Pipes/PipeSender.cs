using System.IO.Pipes;
using System.Text;

namespace Collector
{
    public class PipeSender
    {
        public static void SendOutput(NamedPipeClientStream pipeClient, string output)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(output);
            pipeClient.Write(buffer, 0, buffer.Length);
        }
    }
}
