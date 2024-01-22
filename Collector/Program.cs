using System;
using NLua;
using Collector.Pipes;

namespace Collector
{
    /// <summary>
    /// The main entry point for the Collector application.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Start the named pipe server for communication with the Director
            NamedPipeServer.Start();
        }
    }
}
