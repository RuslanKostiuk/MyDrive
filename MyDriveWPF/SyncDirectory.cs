using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDriveWPF
{
    public static class SyncDirectory
    {
        static List<ServiceReference1.StorageFile> directories;
        static List<string> myDir = new List<string>();
        static void GetDirectories(string root,string base_path)
        {
       
                    myDir.AddRange(Directory.GetDirectories(root).Select(x => x.Remove(0,base_path.Length)));

                    for (int i = 0; i < Directory.GetDirectories(root).Length; i++)
                    {
                        GetDirectories(Directory.GetDirectories(root)[i],base_path);
                    }
         
            }


        public static void Synchronize(string root, string base_path)
        {
            
            
                Task.Run(() =>
                {
                    lock (new object())
                    {
                        directories = new ServiceReference1.StorrageServiceClient().SearchDirectories(root.Remove(0, base_path.Length)).Files.ToList();
                        GetDirectories(root, base_path);


                        for (int i = 0; i < myDir.Count; i++)
                        {
                            if (!directories.Select(x => x.Name).ToList().Contains(myDir[i]))
                            {
                                Directory.Delete(base_path + myDir[i]);
                            }
                        }

                        for (int i = 0; i < directories.Count; i++)
                        {
                            if (!myDir.Contains(directories[i].Name))
                            {
                                Directory.CreateDirectory(base_path + directories[i].Name);
                            }
                        }
                    }
                    Thread.Sleep(1000 * 60);
                });
            
        }
    }
}
