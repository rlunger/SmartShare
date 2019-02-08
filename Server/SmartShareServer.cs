using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Core.Dto;

namespace Server
{
    class Program
    {
        static void Main (string[] args)
        {
            var tcpListener = new TcpListener (
                IPAddress.Parse (Core.Config.ServerIpAddress),
                Core.Config.ServerPortNumber
            );

            tcpListener.Start ();
            Directory.CreateDirectory (Server.Config.StorageRoot);
            while (true)
            {
                Console.WriteLine ("Waiting for connection...");
                var tcpClient = tcpListener.AcceptTcpClient ();
                Task.Run (() => HandleClient (tcpClient));
            }
        }

        static void HandleClient (TcpClient client)
        {
            using (var stream = client.GetStream ())
            {
                var payloadSerializer = new XmlSerializer (typeof (Payload));
                var inbound = (Payload) payloadSerializer.Deserialize (stream);
                var outbound = new Payload ();
                var context = new SmartShareContext ();

                //Client requested upload authorization.
                if (inbound.Status == Core.Config.RequestUpload)
                {
                    HandleUploadRequest (inbound, outbound);
                }

                //Client requested download.
                else if (inbound.Status == Core.Config.RequestDownload)
                {
                    HandleDownloadRequest (inbound, outbound);
                }

                //Client requested file info.
                else if (inbound.Status == Core.Config.RequestInfo)
                {
                    HandleInfoRequest (inbound, outbound);
                }

                //Client passed authorization. Create file and update record.
                else
                {
                    HandleUploadFullfillment (inbound, outbound);
                }

                //Send response to client.
                payloadSerializer.Serialize (stream, outbound);
                client.Close ();
            }
        }

        public static void HandleDownloadRequest (Payload inbound, Payload outbound)
        {
            var context = new SmartShareContext ();
            var queryResult = from s in context.Storage
            where s.Filename.Contains (inbound.Filename)
            select s;

            //File doesn't exist or user doesn't have correct password.
            if (queryResult.Count () == 0 || queryResult.First ().Password != inbound.Password)
            {
                outbound.Status = Core.Config.ServerError;
            }

            //File exists and user has correct password.
            else
            {
                var fileRecord = queryResult.First ();
                //File hasn't expired or reached max downloads.
                //Return file and update downloads left.
                if (fileRecord.TimeExpiring > DateTime.Now
                    && fileRecord.DownloadsRemaining != 0)
                {
                    outbound.Base64FileData = Core.Util.FileToBase64String (
                        Server.Config.StorageRoot + fileRecord.FileHash
                    );
                    outbound.Status = Core.Config.ServerSuccess;

                    if (fileRecord.DownloadsRemaining != Core.Config.OptionUnlimitedDownload)
                    {
                        fileRecord.DownloadsRemaining--;
                    }

                    context.Storage.Update (fileRecord);
                }

                //File has expired or reache max downloads.
                //Delete file and update record.
                else
                {
                    outbound.Status = Core.Config.ServerError;
                    File.Delete (Server.Config.StorageRoot + fileRecord.FileHash);
                    context.Storage.Remove (fileRecord);
                }

                context.SaveChanges ();
            }
        }

        public static void HandleUploadRequest (Payload inbound, Payload outbound)
        {
            var context = new SmartShareContext ();
            var queryResult = from s in context.Storage
            where s.Filename.Contains (inbound.Filename)
            select s;

            if (queryResult.Count () == 0)
            {
                //File doesn't exist, add initial entry.
                var newFile = new StorageModel ();
                newFile.Filename = inbound.Filename;
                newFile.FileHash = Guid.NewGuid ().ToString ();
                newFile.DownloadsRemaining = (inbound.DownloadsLeft > 0)
                    ? inbound.DownloadsLeft
                    : Core.Config.DefaultDownloadLimit;
                newFile.Password = inbound.Password;
                //newFile.TimeCreated and newFile.TimeExpiring will be added
                //upon file receipt.
                context.Storage.Add (newFile);
                context.SaveChanges ();

                //Prepare authorization response.
                outbound.Status = newFile.FileHash;
                if (inbound.TimeLeft < Core.Config.MinLifetime)
                {
                    outbound.TimeLeft = Core.Config.DefaultLifetime;
                }

                else if (inbound.TimeLeft > Core.Config.MaxLifetime)
                {
                    outbound.TimeLeft = Core.Config.MaxLifetime;
                }

                else
                {
                    outbound.TimeLeft = inbound.TimeLeft;
                }
            }

            else
            {
                outbound.Status = Core.Config.ServerError;
            }
        }

        public static void HandleUploadFullfillment (Payload inbound, Payload outbound)
        {
            var context = new SmartShareContext ();
            var queryResult = from s in context.Storage
            where s.FileHash.Contains (inbound.Status)
            select s;

            //File GUID not found in database.

            if (queryResult.Count () == 0)
            {
                outbound.Status = Core.Config.ServerError;
            }

            //GUID found.
            else
            {
                var updatedFile = queryResult.First ();
                updatedFile.TimeCreated = DateTime.Now;
                updatedFile.TimeExpiring = updatedFile.TimeCreated + inbound.TimeLeft;
                context.Storage.Update (updatedFile);
                context.SaveChanges ();
                Core.Util.WriteBase64StringToFile (
                    inbound.Base64FileData,
                    Server.Config.StorageRoot + updatedFile.FileHash
                );
                outbound.Status = Core.Config.ServerSuccess;
            }
        }

        public static void HandleInfoRequest (Payload inbound, Payload outbound)
        {
            var context = new SmartShareContext ();
            var queryResult = from s in context.Storage
            where s.Filename.Contains (inbound.Filename)
            select s;

            //File not found or incorrect password provided.
            if (queryResult.Count () == 0 || queryResult.First ().Password != inbound.Password)
            {
                outbound.Status = Core.Config.ServerError;
            }

            //File found and correct password provided.
            else
            {
                var fileRecord = queryResult.First ();

                //File not expired or over download limit.
                //Return file info.
                if (fileRecord.TimeExpiring > DateTime.Now
                    && fileRecord.DownloadsRemaining != 0)
                {
                    outbound.TimeCreated = fileRecord.TimeCreated;
                    outbound.TimeLeft = fileRecord.TimeExpiring - DateTime.Now;
                    outbound.DownloadsLeft = fileRecord.DownloadsRemaining;
                    outbound.Status = Core.Config.ServerSuccess;
                }

                //File expired or over download limit.
                //Delete file.
                else
                {
                    outbound.Status = Core.Config.ServerError;
                    File.Delete (Server.Config.StorageRoot + fileRecord.FileHash);
                    context.Storage.Remove (fileRecord);
                    context.SaveChanges ();
                }
            }
        }
    }
}
