using System;
using System.Collections.Generic;

using Client.Options;

using CommandLine;
using static Client.Options.DownloadOptions;
using static Client.Options.UploadOptions;
using static Client.Options.ViewOptions;

namespace Client
{
    public class Program
    {
        private static readonly bool developmentMode = false;

        public static void Main (string[] args)
        {
            if (args.Length <= 0 && developmentMode)
            {
                Tester.RunTestArgs ();
            }
            else
            {
                RunCommandArgs (args);
            }
        }

        public static void RunCommandArgs (string[] args)
        {
            Parser.Default.ParseArguments<DownloadOptions, UploadOptions, ViewOptions> (args)
                .MapResult (
                    (DownloadOptions opts) => ExecuteDownloadAndReturnExitCode (opts),
                    (UploadOptions opts) => ExecuteUploadAndReturnExitCode (opts),
                    (ViewOptions opts) => ExecuteViewAndReturnExitCode (opts),
                    errs => 1);
        }
    }
}
