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
        // TODO: Add Warning. これ、失敗すると起動しなくなるので、注意が必要！
        Environment.SetEnvironmentVariable($"{_args[1]}", $"{_args[2]}", EnvironmentVariableTarget.Machine);
    }
}
