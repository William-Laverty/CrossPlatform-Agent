-- Function to retrieve operating system details
function getOS()
    local os_name = nil
    local machine_name = nil
    local logged_user = nil

    -- Check if the path separator indicates Windows
    if package.config:sub(1, 1) == '\\' then
        os_name = 'Windows'
    else
        local handle = io.popen('uname')
        local uname = handle:read('*a')
        handle:close()

        if uname == 'Darwin\n' then      
            os_name = 'macOS'
        elseif uname == 'Linux\n' then
            os_name = 'Linux'
        else
            local os_env = os.getenv('OS')
            if os_env and os_env:match("^Windows") then
                os_name = 'Windows'
            else
                os_name = 'Unknown'
            end
        end
    end

    -- Get machine name and logged user details
    machine_name = os.getenv('COMPUTERNAME') or os.getenv('HOSTNAME') or 'Unknown'
    logged_user = os.getenv('USERNAME') or os.getenv('USER') or 'Unknown'

    return {
        os_name = os_name,
        machine_name = machine_name,
        logged_user = logged_user
    }
end

-- Define the path to the dkjson.lua file
local dkjson_path = "./dkjson.lua"

-- Function to check if a file exists
function file_exists(file)
    local f = io.open(file, "rb")
    if f then
        f:close()
        return true
    end
    return false
end

-- Function to download dkjson.lua based on OS
function downloadDkjson()
    local os_name = getOS().os_name
    local url = "https://raw.githubusercontent.com/LuaDist/dkjson/master/dkjson.lua"

    if os_name == 'Windows' then
        local powershell_command = 'powershell -Command "(New-Object System.Net.WebClient).DownloadFile(\'' .. url .. '\', \'' .. dkjson_path .. '\')"'
        os.execute(powershell_command)
    elseif os_name == 'macOS' or os_name == 'Linux' then
        local curl_command = "curl -s -o " .. dkjson_path .. " " .. url
        os.execute(curl_command)
    else 
        print("Unsupported operating system for dkjson download.")
    end
end

-- Check if dkjson.lua exists
if not file_exists(dkjson_path) then
    print("Downloading dkjson.lua...")
    downloadDkjson()
    print("dkjson.lua downloaded.")
end

-- Retrieve OS details
local os_details = getOS()

-- Load dkjson library
local json = require("dkjson")

-- Encode OS details to JSON
local json_data = json.encode(os_details)

-- Write JSON to file
local file = io.open("os_details.json", "w")
file:write(json_data)
file:close()

-- Print a message indicating successful write and the path to the JSON file
print("LUA: OS details (" .. json_data .. ") written to: " .. io.popen('pwd'):read('*l') .. "/os_details.json")
