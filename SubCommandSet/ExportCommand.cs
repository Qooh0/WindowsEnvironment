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
        Console.WriteLine(
            Environment.GetEnvironmentVariable($"{_args[1]}", EnvironmentVariableTarget.Machine)
        );
    }

    private void ParamerterAnalyze()
    {
        for (int i = 1; i < _args.Length; i++)
        {
            Console.WriteLine($"{_args[i]}");
        }
    }
}
