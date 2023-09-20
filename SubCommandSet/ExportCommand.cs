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
        if (showHelp) {
            ShowHelp();
            Environment.Exit(0);
        }
        Console.WriteLine($"{_args.Length}");
        
        if (string.IsNullOrEmpty(envName) && string.IsNullOrEmpty(nameListFile))
        {
            Console.WriteLine("No order");
            return;
        }
        
            Console.WriteLine($"envName : {envName}");
            Console.WriteLine($"nameListFile : {nameListFile}");

        if (string.IsNullOrEmpty(envName) == false) 
        {
            Console.WriteLine("Only stdout env");
            Console.WriteLine(GetRawVal(envName));            
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

    string outputFilename = string.Empty;
    string nameListFile = string.Empty;
    string envName = string.Empty;
    bool useMachineEnvironment = false;
    bool showHelp = false;
    bool verbose = false;

    private void ParamerterAnalyze()
    {
        for (int i = 1; i < _args.Length; i++)
        {
            switch(_args[i])
            {
                case "-f":
                case "--nameListFile":
                    nameListFile = _args[++i];
                    break;
                case "-o":
                case "--outputFile":
                    outputFilename = _args[++i];
                    break;
                case "-m":
                case "--machine":
                    useMachineEnvironment = true;
                    break;
                case "-h":
                case "--help":
                    showHelp = true;
                    break;
                case "-v":
                case "--verbose":
                    verbose = true;
                    break;
                default:
                    envName = _args[i];     // STDIN
                    break;
            }
        }
    }

    private IEnumerable<string> ReadFile()
    {
        string[] lines = File.ReadAllLines(nameListFile);
        foreach (string line in lines)
        {
            yield return line;
        }
    }

    private string? GetRawVal(string variableName)
    {
        if (verbose)
        {
            Console.WriteLine($"Environment Variable Key : {envName}\nCommand Params -o {this.outputFilename}, -f {this.nameListFile}, -m {useMachineEnvironment}, -h {showHelp}");
        }

        if (useMachineEnvironment)
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
        }
        else
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
        }
    }

}
