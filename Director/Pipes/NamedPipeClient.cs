using System;
using System.IO.Pipes;
using System.Threading;
using System.Diagnostics;
using Director.Logs;

namespace Director.Pipes
{
    /// <summary>
    /// Represents a named pipe client for communication with the Collector.
    /// </summary>
    public class NamedPipeClient
    {
        /// <summary>
        /// Runs the named pipe client to communicate with the Collector.
        /// </summary>
        public static void RunClient()
        {
            // Specify the name of the named pipe
            string pipeName = "AgentPipe";

            // Variable to store received data
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
                            Logger.Log("DIRECTOR: Connected to Collector.");

                            // Read the data from the named pipe
                            data = PipeReader.ReadDataFromPipe(pipeClient);

                            Logger.Log("DIRECTOR: Received data from Collector:");
                            Logger.Log(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"DIRECTOR: Error connecting to Collector: {ex.Message}");

                        // Perform update check with the received data
                        UpdateCheck.ExtractHasDataUpdated(data);

                        if (!pipeClient.IsConnected)
                        {
                            Logger.Log("DIRECTOR: Executing collector");

                            // Start the Collector process
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
