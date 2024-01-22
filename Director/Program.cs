using System;
using Director.Pipes;

namespace Director
{
    /// <summary>
    /// The main entry point for the Director application.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Run the named pipe client to communicate with the Collector
            NamedPipeClient.RunClient();
        }
    }
}
