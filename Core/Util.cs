using System;
using System.IO;

namespace Core
{
    public class Util
    {
        public static string FileToBase64String (string path)
        {
            return Convert.ToBase64String (File.ReadAllBytes (path));
        }

        public static void WriteBase64StringToFile (string b64, string path)
        {
            File.WriteAllBytes (path, Convert.FromBase64String (b64));
        }
    }
}
