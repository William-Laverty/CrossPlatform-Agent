using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Collector.Logs;
using Collector.Scripts;

namespace Collector.Pipes
{
    /// <summary>
    /// Represents a named pipe server for communication with the Director.
    /// </summary>
    public class NamedPipeServer
    {
        private static int numThreads = 1;

        /// <summary>
        /// Starts the named pipe server on a separate thread.
        /// </summary>
        public static void Start()
        {
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        /// <summary>
        /// The main thread function for handling communication with the Director.
        /// </summary>
        private static void ServerThread()
        {
            // Create an instance of LuaHandler to handle Lua scripting
            LuaHandler luaHandler = new LuaHandler();

            // Create a named pipe server stream
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("AgentPipe", PipeDirection.InOut, numThreads);

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            // Log the connection information
            Logger.Log("COLLECTOR: Director connected.");
            Logger.Log($"COLLECTOR: Username: {Environment.UserName}");
            bool dataHasUpdated = false;

            try
            {
                // Create a StreamString to simplify reading and writing strings to the pipe
                StreamString ss = new StreamString(pipeServer);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Execute the 'w' command to get user sessions information || MACOS OR LINUX
                    string userSessionsOutput = UserSessions.Execute("w");
                    ss.WriteString(userSessionsOutput);
                    Logger.Log($"OPERATING SYSTEM: MacOSX");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Execute the 'query session' command to get user sessions information || WINDOWS
                    string userSessionsOutput = UserSessions.Execute("query session");
                    ss.WriteString(userSessionsOutput);
                } 

                // Send the flag indicating whether data has been updated
                string dataUpdate = dataHasUpdated.ToString();
                ss.WriteString(dataUpdate);
                dataHasUpdated = false;
            }
            catch (IOException e)
            {
                // Log any IOException that occurs during communication
                Logger.Log($"COLLECTOR: ERROR: {e.Message}");
            }
            finally
            {
                // Close the named pipe server
                pipeServer.Close();
            }
        }
    }
}
