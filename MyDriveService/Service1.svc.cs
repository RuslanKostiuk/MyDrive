using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyDriveService
{
    
    public class AccessService : IAccessService,IStorrageService
    {
        static string connStr = ConfigurationManager.ConnectionStrings["MyDriveCS"].ConnectionString;
        MyDriveDB db = new MyDriveDB(connStr);
        public AnswerUserResponse UserAuth(string login, string password)
        {
            var _user = db.Users.Where(user => user.Login == login && user.Password == password);
            if (
                  _user.Count() > 0
              ) return AnswerResponceSetter.SetUserResponse(
                  AnswerCode.Complete,
                 _user.First(),
                 "You entered succesfuly"
                  );

            return AnswerResponceSetter.SetUserResponse(
                  AnswerCode.Failed,
                 "authentication failed"
                  );
        }

        public AnswerUserResponse UserRegistration(User user)
        {
            try
            {
                var _user = db.Users.Add(user);
                db.SaveChanges();
                Directory.CreateDirectory(@"C:\Users\Ruslanchik\Desktop\DriveRepositiry/"+ user.Login); 
                return AnswerResponceSetter.SetUserResponse(
                    AnswerCode.Complete,
                    _user,
                    "Registration complete succesfuly"
                    );
            }
            catch(Exception ex)
            {
                return AnswerResponceSetter.SetUserResponse(AnswerCode.Failed, ex.Message);
            }
        }

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
                return AnswerResponceSetter.SetResponse(
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

                return AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
            }
        }

        public AnswerResponse Delete(string name)
        {
            try
            {
                File.Delete(name);
                return AnswerResponceSetter.SetResponse(AnswerCode.Complete, "File deleted succesfuly");

            }
            catch (Exception ex)
            {
                return AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
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
            return AnswerResponceSetter.SetResponse(
                    AnswerCode.Complete,
                    new StorageFile()
                    {
                        Bytes = File.ReadAllBytes(path).ToList(),
                        Name = path
                    },
                    "All data in your disk read succesfuly"
                );
        }

        public AnswerResponse Search(string path)
        {
            List<string> data = new List<string>();
            List<StorageFile> files = new List<StorageFile>();
            this.FindData(path, data);

            for (int i = 0; i < data.Count(); i++)
            {
                files.Add(
                        new StorageFile()
                        {
                            Name = data[i],
                        });
            }
            return AnswerResponceSetter.SetResponse(
                AnswerCode.Complete,
                files,
                "All files read succesfuly"
                );
        }

        public AnswerResponse ReadAll(string path)
        {

            List<StorageFile> files = new List<StorageFile>();
            for(int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                files.Add(new StorageFile()
                {
                    Name = Directory.GetDirectories(path)[i]
                });
            }
            
            return AnswerResponceSetter.SetResponse(
               AnswerCode.Complete,
               files,
               "All files read succesfuly"
               );
        }


        public AnswerResponse Update(List<byte> file, string path)
        {
            return this.Create(file, path);
        }


        void FindData(string path, List<string> data)
        {

            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                data.Add(Directory.GetFiles(path)[i]);
            }

            for (int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                data.Add(Directory.GetDirectories(path)[i]);
                FindData(Directory.GetDirectories(path)[i], data);
            }

        }

        public AnswerResponse OpenFolder(string path)
        {
            try
            {
                List<StorageFile> folders = new List<StorageFile>();
                Directory.GetDirectories(path).ToList().ForEach(
                    directory => folders.Add(new StorageFile() { Name = directory })
                    );

                return AnswerResponceSetter.SetResponse(
                    AnswerCode.Complete,
                    folders,
                    "folders in folder"
                    );
            }catch(Exception ex)
            {
                return AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
            }
        }
    }





}
