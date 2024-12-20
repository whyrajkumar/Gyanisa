using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Comman
{
    public class MainModule
    {
        public static string contentRoot { get; set; }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        public static void DeleteFile(string target_dir)
        {
            if (System.IO.File.Exists(target_dir)==true)
            {
                File.SetAttributes(target_dir, FileAttributes.Normal);
                File.Delete(target_dir);
            }
            
        }
       



    }
}
