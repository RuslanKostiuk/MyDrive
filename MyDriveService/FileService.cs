using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyDriveService
{
    public class StorageFile
    {
        public StorageFile()
        {
            Bytes = new List<byte>();
        }
        public string Name { get; set; }
        public List<byte> Bytes { get; set; }
    }

    public class StorageService : IStorrageService
    {

        AnswerResponse answer = new AnswerResponse();

        public AnswerResponse Create(List<byte> file, string path)
        {

            try
            {
                string file_name = Path.GetFileName(path);
                string directory_path = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory_path)) Directory.CreateDirectory(directory_path);
                FileStream stream = File.Create(file_name);
                stream.Write(file.ToArray(), 0, file.Count);
                stream.Dispose();
                return answer.SetResponse(
                    AnswerCode.Complete,
                    new StorageFile()
                    {
                        Bytes = file,
                        Name = path,
                    },
                   "File added succesfuly"
               );

            }
            catch (Exception ex)
            {

                return answer.SetResponse(AnswerCode.Failed, ex.Message);
            }
        }

        public AnswerResponse Delete(string name)
        {
                try
                {
                    File.Delete(name);
                    return answer.SetResponse(AnswerCode.Complete, "File deleted succesfuly");

                }
                catch (Exception ex)
                {
                  return answer.SetResponse(AnswerCode.Failed, ex.Message);
                }
        }
        //TODO: Implemets check user role ans send all space
        public int GetAllSpace(int id)
        {
            throw new NotImplementedException();
        }
        //TODO: Implemets check free space
        public int GetFreeSpace(int id)
        {
            throw new NotImplementedException();
        }

        public AnswerResponse Read(string path)
        {
            return answer.SetResponse(
                    AnswerCode.Complete,
                    new StorageFile()
                    {
                        Bytes = File.ReadAllBytes(path).ToList(),
                        Name = path
                    },
                    "All data in your disk read succesfuly"
                );
        }

        public AnswerResponse ReadAll(string path)
        {
            List<string> data = new List<string>();
            List<StorageFile> files = new List<StorageFile>();
            this.FindFiles(path, data);

            for (int i = 0; i < data.Count(); i++)
            {
                files.Add(
                        new StorageFile()
                        {
                            Name = data[i]
                        });
            }
            return answer.SetResponse(
                AnswerCode.Complete,
                files,
                "All files read succesfuly"
                );
        }


        public AnswerResponse Update(List<byte> file, string path)
        {
            return this.Create(file, path);
        }


        void FindFiles(string path,List<string> data)
        {

            for(int i = 0; i < Directory.GetFiles(path).Length;i++)
            {
                data.Add(Directory.GetFiles(path)[i]);
            }

            for(int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                FindFiles(Directory.GetDirectories(path)[i], data);
            }

        }
    }
}