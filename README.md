# Cross Platform Agent

## Overview

The Agent is a cross-platform .NET 6 project that enables communication between the Collector and Director components through named pipes. The Collector gathers system information using Lua scripting, and the Director initiates and manages the communication.

## Components

1. **Collector**
   - Collects system information using Lua scripts.
   - Sends data to the Director through a named pipe.

2. **Director**
   - Manages communication with the Collector.
   - Receives and processes system information.
   - Initiates the Collector if not connected.

## Project Structure

- **Collector**
  - `LuaHandler.cs`: Handles the execution of Lua scripts for gathering system information.
  - `PipeSender.cs`: Sends output through a named pipe to the Director.
  - `NamedPipeServer.cs`: Listens for connections from the Director and handles data transfer.
  - `UserSessions.cs`: Executes commands related to user sessions and extracts information.

- **Director**
  - `NamedPipeClient.cs`: Communicates with the Collector through a named pipe.
  - `PipeReader.cs`: Reads data from the named pipe.
  - `UpdateCheck.cs`: Extracts information related to data updates.
  - `StreamString.cs`: Helper class for reading and writing strings to a stream.

- **Scripts**
  - `os_info_fetch.lua`: Lua script for collecting OS information.
  - `dkjson.lua`: Lua script for encoding JSON data.

- **Logs**
  - `Logger.cs`: Provides basic logging functionality.

## Processes Involved

1. **Collector Process**
   - The Collector process is responsible for gathering system information using Lua scripting.
   - It executes the `os_info_fetch.lua` script to collect details such as OS name, machine name, and logged-in user.
   - The collected information is then sent to the Director through a named pipe using the `PipeSender` class.

2. **Director Process**
   - The Director process manages communication with the Collector.
   - It establishes a connection with the Collector through the named pipe using the `NamedPipeClient` class.
   - The Director receives and processes system information, including user session details and OS information.
   - It utilizes the `UpdateCheck` class to extract information related to data updates.

## How to Run

1. Compile the agent by building the project. Two separate executable binaries will be produced.

2. Execute the Director file, which will cause the Director to connect to and run the Collector through the named pipe. It will exchange data and manage communication.

## Dependencies

- **NLua**: Used for embedding Lua scripting capabilities in C#.