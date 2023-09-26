using QadiffWindowsEnvironmentManager.SubCommandSet;

namespace QadiffWindowsEnvironmentManager
{
    /// <summary>
    /// SubCommands.
    /// 
    /// Rule: Only the first letter is capitalized.The rest is lower case.
    /// 
    /// Rule: 最初だけ大文字。あとは小文字でメソッドを定義すること。
    /// 実際のサブコマンドは、ここの関数名
    /// </summary>
    public class SubCommands
    {
        public void Export(string[] args)
        {
            ISubCommand command = new ExportCommand(args);
            command.Execute();
        }

        public void Set(string[] args)
        {
            ISubCommand command = new SetCommand(args);
            command.Execute();
        }
    }
}