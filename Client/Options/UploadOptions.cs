using System;
using System.Collections.Generic;
using System.IO;

using Client.Utils;

using CommandLine;

namespace Client.Options
{
  [Verb (Client.Config.UploadVerb, HelpText = Client.Config.UploadHelpText)]
    public class UploadOptions
    {
        [Option (
            Client.Config.AvailableTimeSingleCharFlag, 
            Client.Config.AvailableTimeFullFlag, 
            HelpText = Client.Config.AvailableTimeFlagHelpText, 
            Required = Client.Config.AvailableTimeFlagRequired
        )]
        public int MaxMinutes
        {
            get;
            set;
        }

        [Option (
            Client.Config.MaxDownloadsSingleCharFlag,
            Client.Config.MaxDownloadsFullFlag, 
            HelpText = Client.Config.MaxDownloadsFlagHelpText,
            Required = Client.Config.MaxDownloadsFlagRequired
        )]
        public int MaxDownloads
        {
            get;
            set;
        }

        [Value (
            0, 
            MetaName = Client.Config.UploadFileListName, 
            HelpText = Client.Config.UploadFileListHelpText, 
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
            Required = Client.Config.UploadPasswordFlagRequired
        )]
        public string Password
        {
            get;
            set;
        } = PasswordGenerator.Generate ();

        public static int ExecuteUploadAndReturnExitCode (UploadOptions options)
        {
            foreach (var filename in options.Filenames)
            {
                var file = new FileInfo (filename);
                if (!file.Exists)
                {
                    Console.WriteLine (Client.Config.MessageFileNotFound, filename);
                    return Client.Config.ResultFailure;
                }
            }

            return (Api.Upload (options))
                ? Client.Config.ResultSuccess
                : Client.Config.ResultFailure;
        }
    }
}
