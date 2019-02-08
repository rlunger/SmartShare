using System;

namespace Core.Dto
{
    public class Payload
    {
        public string Filename { get; set; }
        public string Password { get; set; }
        public TimeSpan TimeLeft { get; set; } = TimeSpan.Zero;
        public string Status { get; set; }
        public DateTime TimeCreated { get; set; }
        public int DownloadsLeft { get; set; }
        public string Base64FileData { get; set; }
        
    }
}