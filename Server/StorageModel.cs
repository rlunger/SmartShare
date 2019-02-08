using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Server
{
    [Table("storage")]
    public class StorageModel
    {
        [Key]
        [Column("file_name")]
        public string Filename { get; set; }

        [Column("time_created")]
        public DateTime TimeCreated { get; set; }

        [Column("time_expiring")]
        public DateTime TimeExpiring { get; set; }

        [Column("downloads_left")]
        public int DownloadsRemaining { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("file_hash")]
        public string FileHash { get; set; }
    }
}