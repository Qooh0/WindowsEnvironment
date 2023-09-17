using System;

System.Console.WriteLine(
Environment.GetEnvironmentVariable("YourVariableName", EnvironmentVariableTarget.Machine)
);
// Environment.SetEnvironmentVariable("YourVariableName", "YourValue", EnvironmentVariableTarget.Machine);

