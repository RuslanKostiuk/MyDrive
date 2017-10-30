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
        public AnswerResponse Create(List<byte> file, string path)
        {
            AnswerResponse answer = new AnswerResponse();
            try
            {
                string file_name = Path.GetFileName(path);
                string directory_path = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory_path)) Directory.CreateDirectory(directory_path);
                FileStream stream = File.Create(file_name);
                stream.Write(file.ToArray(), 0, file.Count);
                stream.Dispose();
                answer.Code = AnswerCode.Complete;
                answer.Files.Add(new StorageFile()
                {
                    Bytes = file,
                    Name = path,
                });
                answer.Message = "File added succesfuly";
                return answer;
            }
            catch(Exception ex)
            {
                answer.Code = AnswerCode.Failed;
                answer.Message = ex.Message;
                return answer;
            }
        }

        public AnswerResponse Delete(string name)
        {
            throw new NotImplementedException();
        }

        public int GetAllSpace(int id)
        {
            throw new NotImplementedException();
        }

        public int GetFreeSpace(int id)
        {
            throw new NotImplementedException();
        }

        public AnswerResponse Read(string path)
        {
            throw new NotImplementedException();
        }

        public AnswerResponse ReadAll(string path)
        {
            throw new NotImplementedException();
        }

        public AnswerResponse Update(List<byte> file, string path)
        {
            throw new NotImplementedException();
        }
    }
}