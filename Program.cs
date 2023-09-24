using System.Reflection.Metadata;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Data;

namespace QadiffWindowsEnvironmentManager
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                Environment.Exit(0);
            }

            string firstArgument = args[0];     // for code reading. 読みやすくするため

            SubCommands _sc = new SubCommands();
            Type _subCommandClassInstance = _sc.GetType();
            MethodInfo[] _subcommandMethods = _subCommandClassInstance.GetMethods();
            string[] _subcommands = new string[_subcommandMethods.Length];
            for (int i = 0; i < _subcommandMethods.Length; i++)
            {
                _subcommands[i] = _subcommandMethods[i].Name;
            }

            if (_subcommands.Any(v => v.ToLower() == firstArgument.ToLower()))
            {
                MethodInfo? subCommand = _subCommandClassInstance.GetMethod(CapitalizeFirstLetter(firstArgument));
                if (subCommand is null) {
                    Console.Error.WriteLine("If you get this error, Please teach me! error No. 01");
                }
                subCommand?.Invoke(_sc, new object[] {args});
                Environment.Exit(0);
            }

            if (firstArgument.StartsWith("--") || firstArgument.StartsWith("-"))
            {
                Console.WriteLine($"Option Arg 1: {firstArgument}");
                Environment.Exit(0);
            }

            System.Console.WriteLine(
                Environment.GetEnvironmentVariable("YourVariableName", EnvironmentVariableTarget.Machine)
            );
            // Environment.SetEnvironmentVariable("YourVariableName", "YourValue", EnvironmentVariableTarget.Machine);        

            ShowHelp();
        }

        private static void ShowHelp()
        {
            Console.WriteLine(
            """
            set/backup Environment variable.

            windowsEnvironment <option>

            -h, help : Show this.
            export   : SubCommand.      Export environment variable.
            """);
        }

        private static string CapitalizeFirstLetter(string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
