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

        if (_envVarName.StartsWith('-')) {
            ShowHelp();
            Environment.Exit(0);
        }

        Dictionary<string, string> envKeyValueDict = [];

        // input
        if (string.IsNullOrEmpty(_envVarName) == false) 
        {
            envKeyValueDict.Add(_envVarName, GetRawVal(_envVarName) ?? string.Empty);
        }
        else if (string.IsNullOrEmpty(_nameListFile) == false)
        {
            IEnumerable<string> nameList = ReadFile(_nameListFile);
            foreach (string variableName  in nameList)
            {
                string value = GetRawVal(variableName) ?? string.Empty;
                envKeyValueDict.Add(variableName, value);
            }
        }

        // do something


        // output
        Output(envKeyValueDict);
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
            SimpleIniReaderWriter iniFile = new SimpleIniReaderWriter(envKeyValueDict);
            iniFile.Save(_outputFilename);
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
            switch(_args[i].ToLower())
            {
                case "-f":
                case "--namelistfile":
                    _nameListFile = _args[++i];
                    break;
                case "-o":
                case "--outputfile":
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
        if (_verbose)
        {
            Console.WriteLine($"Environment Variable Key : {_envVarName}\nCommand Params -o {this._outputFilename}, -f {this._nameListFile}, -m {_useMachineEnvironment}, -h {_showHelp}");
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

    private string? GetRawVal(string variableName)
    {
        if (_useMachineEnvironment)
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
        }
        else
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
        }
    }
}
