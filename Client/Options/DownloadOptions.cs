using System.Collections.Generic;

using CommandLine;

namespace Client.Options
{
  [Verb (Client.Config.DownloadVerb, HelpText = Client.Config.ViewHelpText)]
    public class DownloadOptions
    {
        [Value (
            0,
            MetaName = Client.Config.DownloadFileListName,
            HelpText = Client.Config.DownloadFileListHelpText,
            Required = true
        )]
        public IEnumerable<string> Filenames
        {
            get;
            set;
        }

        [Option (
            Client.Config.PasswordSingleCharFlag,
            Client.Config.PasswordFullFlag,
            HelpText = Client.Config.PasswordFlagHelpText,
            Required = Client.Config.DownloadPasswordFlagRequired
        )]
        public string Password
        {
            get;
            set;
        }

        public static int ExecuteDownloadAndReturnExitCode (DownloadOptions options)
        {
            return (Api.Download (options))
                ? Client.Config.ResultSuccess
                : Client.Config.ResultFailure;
        }
    }
}
