using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDriveWPF
{
    public static class SyncFile
    {
        static List<ServiceReference1.StorageFile> directories;
        static List<string> myDir = new List<string>();
        static void GetFiles(string root, string base_path)
        {
            lock (root)
            {
                Task.Run(() =>
                {

                    myDir.AddRange(Directory.GetFiles(root).Select(x=>x.Remove(base_path.Length)));


                    for (int i = 0; i < Directory.GetDirectories(root).Length; i++)
                    {
                        GetFiles(Directory.GetDirectories(root)[i], base_path);
                    }
                });
            }
        }

        public static void Synchronize(string root, string base_path)
        {
            directories = new ServiceReference1.StorrageServiceClient().SearchDirectories().Files.ToList();
            GetFiles(root, base_path);


            for (int i = 0; i < directories.Count; i++)
            {
                if (!directories.Select(x => x.Name).ToList().Contains(myDir[i]))
                {
                    Directory.Delete(myDir[i]);
                }
            }

            for (int i = 0; i < myDir.Count; i++)
            {
                if (!myDir.Contains(directories[i].Name))
                {
                    Directory.CreateDirectory(directories[i].Name);
                }
            }
        }
    }
}
