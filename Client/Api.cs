using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Serialization;

using Client.Options;

using Core.Dto;

namespace Client
{
    public class Api
    {
        private Api ()
        {
            throw new InvalidOperationException ();
        }

        /// <summary>
        /// Send download request
        /// </summary>
        /// <param name="">TODO</param>
        /// <returns>true if request was successful and false if unsuccessful</returns>

        public static bool View (ViewOptions options)
        {
            var outbound = new Payload ();
            var inbound = new Payload ();
            var aggregateResult = true;
            foreach (var filename in options.Filenames)
            {
                outbound.Filename = filename;
                outbound.Password = options.Password;
                outbound.Status = Core.Config.RequestInfo;
                var payloadSerializer = new XmlSerializer (typeof (Payload));
                var tcpClient = new TcpClient (Core.Config.ServerIpAddress, Core.Config.ServerPortNumber);
                using (var stream = tcpClient.GetStream ())
                {
                    payloadSerializer.Serialize (stream, outbound);
                    tcpClient.Client.Shutdown (SocketShutdown.Send);
                    inbound = (Payload) payloadSerializer.Deserialize (stream);

                    if (inbound.Status == Core.Config.ServerError)
                    {
                        Console.WriteLine (Client.Config.MessageViewError, filename);
                        tcpClient.Close ();
                        aggregateResult = false;
                    }

                    else
                    {
                        var downloadsLeft = (inbound.DownloadsLeft == Core.Config.OptionUnlimitedDownload)
                            ? Client.Config.StatusDownloadsUnlimited
                            : $"{inbound.DownloadsLeft}";

                        Console.WriteLine (Client.Config.MessageViewSuccess,
                            filename,
                            inbound.TimeCreated,
                            downloadsLeft,
                            inbound.TimeLeft.ToString (Client.Config.TimeFormat)
                        );
                    }
                }

                tcpClient.Close ();
            }

            return aggregateResult;
        }
        public static bool Download (DownloadOptions options)
        {
            var outbound = new Payload ();
            var inbound = new Payload ();
            var aggregateResult = true;
            var fileSuccessCount = 0;
            foreach (var filename in options.Filenames)
            {
                outbound.Filename = filename;
                outbound.Password = options.Password;
                outbound.Status = Core.Config.RequestDownload;
                var xmlSerializer = new XmlSerializer (typeof (Payload));
                var tcpClient = new TcpClient (Core.Config.ServerIpAddress, Core.Config.ServerPortNumber);
                using (var stream = tcpClient.GetStream ())
                {
                    xmlSerializer.Serialize (stream, outbound);
                    tcpClient.Client.Shutdown (SocketShutdown.Send);
                    inbound = (Payload) xmlSerializer.Deserialize (stream);
                    if (inbound.Status == Core.Config.ServerError)
                    {
                        Console.WriteLine (Client.Config.MessageDownloadError, filename);
                        aggregateResult = false;
                    }

                    else
                    {
                        Core.Util.WriteBase64StringToFile (inbound.Base64FileData, filename);
                        Console.WriteLine (Client.Config.MessageDownloadSuccess, filename);
                        fileSuccessCount++;
                    }
                }
                tcpClient.Close ();
            }

            Console.WriteLine (Client.Config.MessageDownloadCount,
                fileSuccessCount,
                options.Filenames.Count ()
            );

            return aggregateResult;
        }

        /// <summary>
        /// Send upload request
        /// </summary>
        /// <param name="">TODO</param>
        /// <returns>true if request was successful and false if unsuccessful</returns>
        public static bool Upload (UploadOptions options)
        {
            var outbound = new Payload ();
            var inbound = new Payload ();
            var aggregateResult = true;
            var fileSuccessCount = 0;
            foreach (var filename in options.Filenames)
            {
                outbound.Filename = Path.GetFileName (filename);
                outbound.Password = options.Password;
                outbound.Status = Core.Config.RequestUpload;
                outbound.DownloadsLeft = options.MaxDownloads;
                outbound.TimeLeft = TimeSpan.FromMinutes (options.MaxMinutes);

                var xmlSerializer = new XmlSerializer (typeof (Payload));
                var tcpClient = new TcpClient (Core.Config.ServerIpAddress, Core.Config.ServerPortNumber);
                string authToken;
                using (var stream = tcpClient.GetStream ())
                {
                    xmlSerializer.Serialize (stream, outbound);
                    tcpClient.Client.Shutdown (SocketShutdown.Send);
                    inbound = (Payload) xmlSerializer.Deserialize (stream);
                    authToken = inbound.Status;
                    if (authToken == Core.Config.ServerError)
                    {
                        Console.WriteLine (Client.Config.MessageUploadError, filename);
                        tcpClient.Close ();
                        aggregateResult = false;
                        continue;
                    }

                    else if (Client.Config.MessageUploadServerAuth != "")
                    {
                        Console.WriteLine (Client.Config.MessageUploadServerAuth, filename);
                    }
                }

                tcpClient.Close ();
                //Let the server decide what these values should be ...
                outbound.TimeLeft = inbound.TimeLeft;
                outbound.DownloadsLeft = inbound.DownloadsLeft;
                outbound.Status = authToken;
                outbound.Base64FileData = Core.Util.FileToBase64String (filename);

                tcpClient = new TcpClient (Core.Config.ServerIpAddress, Core.Config.ServerPortNumber);
                using (var stream = tcpClient.GetStream ())
                {
                    xmlSerializer.Serialize (stream, outbound);
                    tcpClient.Client.Shutdown (SocketShutdown.Send);
                    inbound = (Payload) xmlSerializer.Deserialize (stream);
                    if (inbound.Status == Core.Config.ServerError)
                    {
                        Console.WriteLine (Client.Config.MessageUploadError, filename);
                        aggregateResult = false;
                    }

                    else
                    {
                        Console.WriteLine (Client.Config.MessageUploadSuccess, filename);
                        fileSuccessCount++;
                    }
                }

                tcpClient.Close ();
            }
            Console.WriteLine (Client.Config.MessageUploadCount,
                fileSuccessCount,
                options.Filenames.Count (),
                options.Password
            );

            return aggregateResult;
        }
    }
}
