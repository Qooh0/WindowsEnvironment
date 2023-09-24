using System;
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
        Console.WriteLine($"{_args.Length}");
        
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
        Console.WriteLine("Read from file");
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

    private IEnumerable<string> ReadFile()
    {
        string[] lines = File.ReadAllLines(_nameListFile);
        foreach (string line in lines)
        {
            yield return line;
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

}
