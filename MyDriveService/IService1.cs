using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyDriveService
{
    

    [DataContract]
    public enum AnswerCode{
        [EnumMember] Complete,
        [EnumMember] Failed,
    }

    [MessageContract]
    public class AnswerResponse
    {
        [MessageHeader]
        public AnswerCode Code { get; set; }

        [MessageHeader]
        public string Message { get; set; }

        [MessageBodyMember]
        public List<StorageFile> Files { get; set; }


        public AnswerResponse SetResponse(AnswerCode code, List<StorageFile> files, string message)
        {
   
            this.Code = code;
            this.Files.AddRange(files);
            this.Message = message;
            return this;
        }
       
        public AnswerResponse SetResponse(AnswerCode code, StorageFile file, string message)
        {

            this.Code = code;
            this.Files.Add(file);
            this.Message = message;
            return this;
        }

        public AnswerResponse SetResponse(AnswerCode code, string message)
        {

            this.Code = code;
            this.Message = message;
            return this;
        }
    }


    [ServiceContract]
    public interface IAccessService
    {

  
        [OperationContract]
        AnswerCode UserRegistration(User user);

        //TODO: UserNamePasswordValidator
        [OperationContract]
        AnswerCode UserAuth(string login, string password);

    }

    [ServiceContract]
    public interface IStorrageService
    {

        [OperationContract]
        AnswerResponse Create(List<byte> file,string path);

        [OperationContract]
        AnswerResponse Delete(string name);

        [OperationContract]
        AnswerResponse Update(List<byte> file, string path);

        [OperationContract]
        AnswerResponse Read(string path);

        [OperationContract]
        AnswerResponse ReadAll(string path);

        [OperationContract]
        int GetFreeSpace(int id);

        [OperationContract]
        int GetAllSpace(int id);
    }



}
