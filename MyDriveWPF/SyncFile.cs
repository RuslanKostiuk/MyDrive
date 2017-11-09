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
        static List<ServiceReference1.StorageFile> files;
        static List<string> myFiles = new List<string>();
        static void GetFiles(string root, string base_path)
        {


            myFiles.AddRange(Directory.GetFiles(root).Select(x=>x.Remove(0,base_path.Length)));


                    for (int i = 0; i < Directory.GetDirectories(root).Length; i++)
                    {
                        GetFiles(Directory.GetDirectories(root)[i], base_path);
                    }

        }

        public static void Synchronize(string root, string base_path)
        {
            
            files = new ServiceReference1.StorrageServiceClient().SearchFiles(root.Remove(0, base_path.Length)).Files.ToList();
            GetFiles(root, base_path);


            for (int i = 0; i < myFiles.Count; i++)
            {
                if (!files.Select(x => x.Name).ToList().Contains(myFiles[i]))
                {
                    File.Delete(base_path + myFiles[i]);
                }
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (!myFiles.Contains(files[i].Name))
                {
                    File.WriteAllBytes(base_path + files[i].Name, files[i].Bytes.ToArray());
                }
            }
        }
    }
}
