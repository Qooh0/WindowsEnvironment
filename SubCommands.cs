namespace QadiffWindowsEnvironmentManager
{
    /// <summary>
    /// SubCommands.
    /// 
    /// Rule: Only the first letter is capitalized.The rest is lower case.
    /// 
    /// Rule: 最初だけ大文字。あとは小文字でメソッドを定義すること。
    /// </summary>
    public class SubCommands
    {
        public static void Export()
        {
            Console.WriteLine("Called Export()");
        }

        public static void Set()
        {
            Console.WriteLine("Called Set()");
        }
    }
}