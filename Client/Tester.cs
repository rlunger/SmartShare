using System;
using System.Collections.Generic;

namespace Client
{
    public sealed class Tester
    {
        private static readonly List<string[]> TestArgs = new List<string[]>
        {
            // Experiment with different command line arguments here
            new[] {""},
            new[] {"--help"},
            new[] {"--version"},
            new[] {"upload"},
            new[] {"upload", "*.cs", "bondstone"},
            new[] {"upload", "*.cs", "bondstone"},
            new[] {"download", "Api.cs", "b0ndst0ne"},
            new[] {"download", "Api.cs Program.cs", "bondstone"},
            new[] {"view", "Api.cs Program.cs Config.cs", "bondstone"},
        };

        public static void RunTestArgs()
        {
            int runCount = 1;
            foreach (var args in TestArgs)
            {
                Console.WriteLine($"\n*** RUN #{runCount++}, args: { String.Join(" ", args)}");
                Program.RunCommandArgs(args);
            }
        }
    }
}