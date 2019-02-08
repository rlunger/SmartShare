using System.Collections.Generic;

using CommandLine;

namespace Client.Options
{
  [Verb (Client.Config.ViewVerb, HelpText = Client.Config.ViewHelpText)]
    public class ViewOptions
    {
        [Value (
            0, 
            MetaName = Client.Config.ViewFileListName, 
            HelpText = Client.Config.ViewFileListHelpText, 
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
            Required = Client.Config.ViewPasswordFlagRequired
        )]
        public string Password
        {
            get;
            set;
        }

        public static int ExecuteViewAndReturnExitCode (ViewOptions options)
        {
            return (Api.View (options))
                ? Client.Config.ResultSuccess
                : Client.Config.ResultFailure;
        }
    }
}
