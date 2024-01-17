using System;
using System.IO.Pipes;
using System.Threading;
using System.Diagnostics;

namespace Director.Pipes
{
    public class NamedPipeClient
    {
        public static void RunClient()
        {
            string pipeName = "AgentPipe";
            string data = "";

            while (true)
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In))
                {
                    try
                    {
                        // Connect to the Collector
                        pipeClient.Connect(1000);

                        if (pipeClient.IsConnected)
                        {
                            Console.WriteLine("DIRECTOR: Connected to Collector.");

                            // Read the data from the named pipe
                            data = PipeReader.ReadDataFromPipe(pipeClient);      

                            Console.WriteLine("DIRECTOR: Received data from Collector:");
                            Console.WriteLine(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DIRECTOR: Error connecting to Collector: {ex.Message}");

                        string dataChangeString = UpdateCheck.ExtractHasDataUpdated(data);
                        bool dataChanged = bool.Parse(dataChangeString);

                        if (!pipeClient.IsConnected && dataChanged)
                        {
                            Console.WriteLine("DIRECTOR: Executing collector");
                            Process process = new Process();
                            process.StartInfo.FileName = "./Collector";
                            process.StartInfo.Arguments = "-n";
                            process.Start();
                        }
                    }

                    // Sleep for a short interval before trying again
                    Thread.Sleep(5000);
                }
            }
        }
    }
}