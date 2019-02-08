namespace Client
{
    public class Config
    {
        public static int ResultSuccess { get; } = 0;
        public static int ResultFailure { get; } = -1;
        public static string MessageUploadSuccess { get; } = "File '{0}' uploaded successfully.";
        public static string MessageUploadError{ get; } = "File '{0}' could not be uploaded.";
        public const string MessageUploadCount = "Uploaded {0}/{1} files with password = {2}.";
        public static string MessageDownloadSuccess{ get; } = "File '{0}' downloaded successfully.";
        public static string MessageDownloadError{ get; } = "File '{0}' could not be downloaded.";
        public static string MessageDownloadCount = "Downloaded {0}/{1} file(s).";
        public static string MessageViewSuccess { get; } = @"
File '{0}' info:
    Time Created: {1}
    Downloads Remaining: {2}
    Time until Expiration: {3}
";
        public static string MessageViewError { get; } = "Could not retrieve info for file '{0}'.";
        public const string MessageFileNotFound = "File {0} not found.";
        public static string MessageUploadServerAuth { get; } = "";
        // public const string MessageUploadServerAuth = "Server authorized upload of file '{0}'.";
        public const string DownloadVerb = "download";
        public const string DownloadHelpText = "Downloads a file if the correct password is provided.";
        public const string DownloadFileListName = "filename(s)";
        public const string DownloadFileListHelpText = "The file(s) to download.";

        public const string UploadVerb = "upload";
        public const string UploadHelpText = "Uploads file(s) provided to the server.";
        public const string UploadFileListName = "filename(s)";
        public const string UploadFileListHelpText = "The file(s) to be uploaded.";

        public const string ViewVerb = "view";
        public const string ViewHelpText = "View info on file(s) for which the user has permissions.";
        public const string ViewFileListName = "filename(s)";
        public const string ViewFileListHelpText = "The file(s) to view.";

        public const char PasswordSingleCharFlag = 'p';
        public const string PasswordFullFlag = "password";
        public const string PasswordFlagHelpText = "Locks file(s) with provided password. A password will be generated for you if not provided.";
        public const bool UploadPasswordFlagRequired = false;
        public const bool DownloadPasswordFlagRequired = true;
        public const bool ViewPasswordFlagRequired = true;
        public const char MaxDownloadsSingleCharFlag = 'n';
        public const string MaxDownloadsFullFlag = "downloads";
        public const string MaxDownloadsFlagHelpText = "Maximum number of downloads allowed. (Default is unlimited)";
        public const bool MaxDownloadsFlagRequired = false;
        public const char AvailableTimeSingleCharFlag = 't';
        public const string AvailableTimeFullFlag = "time";
        public const string AvailableTimeFlagHelpText = "Amount of time (in minutes) to serve the file for. (Default is 60)";
        public const bool AvailableTimeFlagRequired = false;
        public const string StatusDownloadsUnlimited = "Unlimited";
        public const string TimeFormat = @"hh\:mm\:ss";


        
    }
}