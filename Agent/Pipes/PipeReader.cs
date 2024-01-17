using System;
using System.IO.Pipes;
using System.Text;

namespace Director.Pipes
{
    public class PipeReader
    {
        public static string ReadDataFromPipe(NamedPipeClientStream pipeClient)
        {
            StringBuilder sb = new StringBuilder();

            using (StreamReader reader = new StreamReader(pipeClient, Encoding.UTF8, true, 4096, true))
            {
                while (!reader.EndOfStream)
                {
                    sb.Append(reader.ReadLine());
                }
            }

            return sb.ToString();
        }
    }
}
