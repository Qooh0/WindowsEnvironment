using System.Diagnostics;

namespace QadiffWindowsEnvironmentManager.SubCommandSet;

public class SetCommand : ISubCommand
{
    private string[] _args;
    public SetCommand(string[] args)
    {
        _args = args;
    }

    public void Execute()
    {
        ParamerterAnalyze();
        if (_showHelp) {
            ShowHelp();
            Environment.Exit(0);
        }

        if(string.IsNullOrEmpty(_envVarValue) && string.IsNullOrEmpty(_file))
        {
            // no args
            ShowHelp();
            return;
        }

        if(string.IsNullOrEmpty(_envVarValue) == false && string.IsNullOrEmpty(_file) == false)
        {
            // both args error
            ShowHelp();
            return;
        }

        Dictionary<string, string> envKeyValueDict = [];
        // from stdin
        if(string.IsNullOrEmpty(_envVarValue) == false)
        {
            envKeyValueDict.Add(_envVarName, _envVarValue);
        }

        // from file
        if(string.IsNullOrEmpty(_file) == false)
        {
            SimpleIniReaderWriter iniFile = new SimpleIniReaderWriter(_file);
            envKeyValueDict = iniFile.Entries();
        }

        // do something
        foreach (string ignoreKey in ReadIgnoreFile())
        {
            foreach (var key in envKeyValueDict.Keys)
            {
                if(key.ToLower() == ignoreKey.ToLower())
                {
                    envKeyValueDict.Remove(key);
                }
            }
        }

        // output
        foreach (KeyValuePair<string, string> kvp  in envKeyValueDict)
        {
            SetRawVal(kvp);
        }
    }

    private void ShowHelp()
    {
        Console.WriteLine(
        """
        set <key> <value>

        set evnironment variable.

        -f --file           : variable name list, separeted by \n.  
        -m --machine        : target is machine. default is "User Environment Variable".
        -d --dry-run        : Dry run. To testing a process without actually executing or implementing it
        """
        );
    }

    string _file = string.Empty;
    string _envVarName = string.Empty;
    string _envVarValue = string.Empty;
    bool _useMachineEnvironment = false;
    bool _showHelp = false;
    bool _dry_run = false;
    bool _verbose = false;

    private void ParamerterAnalyze()
    {
        for (int i = 1; i < _args.Length; i++)
        {
            switch(_args[i])
            {
                case "-f":
                case "--file":
                    _file = _args[++i];
                    break;
                case "-m":
                case "--machine":
                    _useMachineEnvironment = true;
                    break;
                case "-h":
                case "--help":
                    _showHelp = true;
                    break;
                case "-d":
                case "--dry-run":
                    _dry_run = true;
                    break;
                case "-v":
                case "--verbose":
                    _verbose = true;
                    break;
                default:
                    // STDIN
                    if (string.IsNullOrEmpty(_envVarName)) {
                        _envVarName = _args[i];     // STDIN
                    }
                    else if (string.IsNullOrEmpty(_envVarValue)) {
                        _envVarValue = _args[i];
                    }
                    break;
            }
        }
        if (_verbose)
        {
            Console.WriteLine($"Environment Variable Key : {_envVarName}\nCommand Params -f {this._file}, -m {_useMachineEnvironment}, -d {_dry_run}, -h {_showHelp}");
        }
    }

    public void SetRawVal(KeyValuePair<string, string> kvp)
    {
        if(_useMachineEnvironment)
        {
            if(_dry_run)
            {
                Console.WriteLine($"{kvp.Key}={kvp.Value} : MachineEnvironment");
            }
            else
            {
                Environment.SetEnvironmentVariable($"{kvp.Key}", $"{kvp.Value}", EnvironmentVariableTarget.Machine);
            }
        }
        else
        {
            if(_dry_run)
            {
                Console.WriteLine($"{kvp.Key}={kvp.Value} : UserEnvironment");
            }
            else
            {
                Environment.SetEnvironmentVariable($"{kvp.Key}", $"{kvp.Value}", EnvironmentVariableTarget.User);
            }
        }
    }

    public IEnumerable<string> ReadIgnoreFile(string filepath = ".varenvignore")
    {
        string[] lines = File.ReadAllLines(filepath);
        foreach (string line in lines)
        {
            yield return line;
        }
    }
}
