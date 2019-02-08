using System;
namespace Core
{
    public class Config
    {
        public static string ServerIpAddress { get; } = "127.0.0.1";

        public static int ServerPortNumber { get; } = 3000;

        public static string ServerSuccess { get; } = "OK";
        public const string ServerError  = "ERROR";
        public static string RequestDownload { get; } = "DOWNLOAD";
        public static string RequestUpload { get; } = "UPLOAD";
        public static string RequestInfo { get; } = "INFO";

        public static int OptionUnlimitedDownload { get; } = -1;
        public static int DefaultDownloadLimit { get; } = OptionUnlimitedDownload;
        public static TimeSpan DefaultLifetime { get; } = new TimeSpan(1,0,0);
        public static TimeSpan MaxLifetime { get; } = new TimeSpan(24,0,0);
        public static TimeSpan MinLifetime { get; } = new TimeSpan(0,1,0);
        

    }
}