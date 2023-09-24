using System;
using System.Text.Json;
namespace QadiffWindowsEnvironmentManager.SubCommandSet;

public class ExportCommand : ISubCommand
{
    private string[] _args;
    public ExportCommand(string[] args)
    {
        _args = args;
    }
    public void Execute()
    {
        this.ParamerterAnalyze();
        if (_showHelp) {
            ShowHelp();
            Environment.Exit(0);
        }
        
        if (string.IsNullOrEmpty(_envVarName) && string.IsNullOrEmpty(_nameListFile))
        {
            // no args
            return;
        }

        if (string.IsNullOrEmpty(_envVarName) == false && string.IsNullOrEmpty(_nameListFile) == false)
        {
            // both args error
            return;
        }

        if (string.IsNullOrEmpty(_envVarName) == false) 
        {
            Console.WriteLine(GetRawVal(_envVarName));            
            return;
        }
        
        // use nameListFile
        if(string.IsNullOrEmpty(_nameListFile) == false)
        {
            Dictionary<string, string> envKeyValueDict = new ();
            IEnumerable<string> nameList;
            if (IsJsonFormat(_nameListFile)) 
            {
                nameList = ReadJsonFile(_nameListFile);
            }
            else
            {
                nameList = ReadFile(_nameListFile);
            }

            foreach (string variableName  in nameList)
            {
                string value = GetRawVal(variableName) ?? string.Empty;
                envKeyValueDict.Add(variableName, value);
            }
            Output(envKeyValueDict);
        }
    }

    private void Output(Dictionary<string, string> envKeyValueDict)
    {
        if (string.IsNullOrEmpty(_outputFilename))
        {
            foreach (KeyValuePair<string, string> kvp in envKeyValueDict)
            {
                Console.WriteLine($"{kvp.Key}={kvp.Value}");
            }
        }

        if (string.IsNullOrEmpty(_outputFilename) == false)
        {
            using (StreamWriter sw = new StreamWriter(_outputFilename))
            {
                sw.Write(JsonSerializer.Serialize(envKeyValueDict));
            }
        }
    }

    private void ShowHelp()
    {
        Console.WriteLine(
        """
        Export evnironment variable.

        -f --nameListFile   : variable name list, separeted by \n.  
        -o --outputFile     : Export to file.
        -m --machine        : target is machine. default is "User Environment Variable".
        """
        );
    }

    string _outputFilename = string.Empty;
    string _nameListFile = string.Empty;
    string _envVarName = string.Empty;
    bool _useMachineEnvironment = false;
    bool _showHelp = false;
    bool _verbose = false;

    private void ParamerterAnalyze()
    {
        for (int i = 1; i < _args.Length; i++)
        {
            switch(_args[i])
            {
                case "-f":
                case "--nameListFile":
                    _nameListFile = _args[++i];
                    break;
                case "-o":
                case "--outputFile":
                    _outputFilename = _args[++i];
                    break;
                case "-m":
                case "--machine":
                    _useMachineEnvironment = true;
                    break;
                case "-h":
                case "--help":
                    _showHelp = true;
                    break;
                case "-v":
                case "--verbose":
                    _verbose = true;
                    break;
                default:
                    _envVarName = _args[i];     // STDIN
                    break;
            }
        }
    }

    private IEnumerable<string> ReadFile(string filepath)
    {
        string[] lines = File.ReadAllLines(filepath);
        foreach (string line in lines)
        {
            yield return line;
        }
    }

    private IEnumerable<string> ReadJsonFile(string filepath)
    {
        // json
        foreach (KeyValuePair<string, string> kvp in JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_nameListFile)))
        {
            yield return kvp.Key;
        }
    }

    private string? GetRawVal(string variableName)
    {
        if (_verbose)
        {
            Console.WriteLine($"Environment Variable Key : {_envVarName}\nCommand Params -o {this._outputFilename}, -f {this._nameListFile}, -m {_useMachineEnvironment}, -h {_showHelp}");
        }

        if (_useMachineEnvironment)
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
        }
        else
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
        }
    }

    private bool IsJsonFormat(string filePath)
    {
        try
        {
            var jsonData = File.ReadAllText(filePath);
            var jsonObject = JsonSerializer.Deserialize<object>(jsonData); // ここでデシリアライズを試みます
            return true;
        }
        catch (JsonException)  // デシリアライズに失敗した場合、例外を捕捉します
        {
            return false;
        }
    }
}
