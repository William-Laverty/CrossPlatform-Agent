using NLua;
using System;
using System.IO;
using Collector; // Add this line to include the Logger namespace
using Collector.Logs;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a handler for executing Lua scripts.
/// </summary>
public class LuaHandler
{
    /// <summary>
    /// Initializes a new instance of the LuaHandler class.
    /// </summary>
    public LuaHandler()
    {
        // Get the directory of the current assembly
        string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "Unknown Path: Executing Assembly";
        string luaScriptPath = "";

        // Combine the current directory with the path to the Lua script
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            luaScriptPath = Path.Combine(currentDirectory, @"Scripts/os_info_fetch.lua");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            luaScriptPath = Path.Combine(currentDirectory, @"Scripts\os_info_fetch.lua");
        }

        // Log the information about Lua script execution
        Logger.Log($"Executing Lua script: {luaScriptPath}");

        // Create a Lua interpreter instance
        using (Lua lua = new Lua())
        {
            try
            {
                // Set the current working directory to the directory of the Lua script
                Directory.SetCurrentDirectory(Path.GetDirectoryName(luaScriptPath) ?? "Unknown Path: Script Directory");

                // Execute the Lua script
                lua.DoFile(luaScriptPath);

                // Execute Lua code to retrieve OS information and ignore the return value
                lua.DoString("return getOS()");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during Lua script execution
                Logger.Log($"Error executing Lua script: {ex.Message}");
            }
        }
    }
}