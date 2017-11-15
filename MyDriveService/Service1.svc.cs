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
        string base_address = @"D:\root\ServerFolder\";
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
                Directory.CreateDirectory(base_address + user.Login); 
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

        public AnswerResponse Create(byte[] file, string path)
        {

            try
            {

                File.Create(base_address + path);
                
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
                if (File.Exists(base_address + name))
                {
                    File.Delete(base_address + name);
                }
                else
                {
                    Directory.Delete(base_address + name);
                }
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
                        Bytes = File.ReadAllBytes(path),
                        Name = path
                    },
                    "All data in your disk read succesfuly"
                );
        }

        public AnswerResponse SearchFiles(string path)
        {
            List<string> data = new List<string>();
            List<StorageFile> files = new List<StorageFile>();
            this.FindFiles(base_address+path, data);


            for (int i = 0; i < data.Count(); i++)
            {
                files.Add(
                        new StorageFile()
                        {
                            Name = data[i],
                            Bytes = File.ReadAllBytes(base_address+ data[i])
                        });
            }

            return AnswerResponceSetter.SetResponse(
                AnswerCode.Complete,
                files,
                "All files read succesfuly"
                );
        }

        public AnswerResponse SearchDirectories(string path)
        {
            List<string> data = new List<string>();
            List<StorageFile> files = new List<StorageFile>();
            this.FindDirectories(base_address+path, data);

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

            for(int i = 0;i< Directory.GetFiles(path).Length; i++)
            {
                files.Add(new StorageFile()
                {
                    Name = Directory.GetFiles(path)[i]
                });
            }

            
            return AnswerResponceSetter.SetResponse(
               AnswerCode.Complete,
               files,
               "All files read succesfuly"
               );
        }


        public AnswerResponse Update(byte[] file, string path)
        {
            try
            {
                bool b = true;
                while (b)
                {
                    try
                    {
                        File.WriteAllBytes(base_address + path, file);
                        b = false;
                    }
                    catch { }
                    
                }
               return AnswerResponceSetter.SetResponse(AnswerCode.Complete, path);
            }catch(Exception ex)
            {
              return  AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
            }
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

        void FindFiles(string path, List<string> data)
        {

            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                data.Add(Directory.GetFiles(path)[i].Remove(0,base_address.Length));
            }

            for (int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                FindFiles(Directory.GetDirectories(path)[i], data);
            }

        }

        void FindDirectories(string path, List<string> data)
        {

            for (int i = 0; i < Directory.GetDirectories(path).Length; i++)
            {
                 
                data.Add(Directory.GetDirectories(path)[i].Remove(0, base_address.Length));
                FindDirectories(Directory.GetDirectories(path)[i], data);
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

        public AnswerResponse CreateFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(base_address+path);
                return AnswerResponceSetter.SetResponse(
                     AnswerCode.Complete,
                     path
                     );
            }
            catch (Exception ex)
            {
                return AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
            }
        }

        public AnswerResponse RenameFolder(string oldPahr, string newPath)
        {
            try
            {
                Directory.Move(base_address + oldPahr, base_address + newPath);
               return AnswerResponceSetter.SetResponse(AnswerCode.Complete, "Folder renamed");
            }catch(Exception ex)
            {
                return  AnswerResponceSetter.SetResponse(AnswerCode.Failed, ex.Message);
            }

        }
    }





}
