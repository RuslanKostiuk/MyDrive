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

    [DataContract]
    public class AnswerResponse
    {
        [DataMember]
        public AnswerCode Code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<StorageFile> Files { get; set; }

        public AnswerResponse()
        {
            Files = new List<StorageFile>();
        }


    }

    public class AnswerUserResponse
    {
        [DataMember]
        public AnswerCode Code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public User _User { get; set; }


    }

    public static class AnswerResponceSetter
    {

        public static AnswerUserResponse SetUserResponse(AnswerCode code, User user, string message)
        {
            AnswerUserResponse response = new AnswerUserResponse();
            response.Code = code;
            response._User = user;
            response.Message = message;
            return response;
        }

        public static AnswerUserResponse SetUserResponse(AnswerCode code,  string message)
        {
            AnswerUserResponse response = new AnswerUserResponse();
            response.Code = code;
            response.Message = message;
            return response;
        }


        public static AnswerResponse SetResponse(AnswerCode code, List<StorageFile> files, string message)
        {
            AnswerResponse response = new AnswerResponse();
            response.Code = code;
            response.Files.AddRange(files);
            response.Message = message;
            return response;
        }

        public static AnswerResponse SetResponse(AnswerCode code, StorageFile file, string message)
        {
            AnswerResponse response = new AnswerResponse();
            response.Code = code;
            response.Files.Add(file);
            response.Message = message;
            return response;
        }

        public static AnswerResponse SetResponse(AnswerCode code, string message)
        {
            AnswerResponse response = new AnswerResponse();
            response.Code = code;
            response.Message = message;
            return response;
        }
    }


    [ServiceContract]
    public interface IAccessService
    {

  
        [OperationContract]
        AnswerUserResponse UserRegistration(User user);

        //TODO: UserNamePasswordValidator
        [OperationContract]
        AnswerUserResponse UserAuth(string login, string password);

    }

    [ServiceContract]
    public interface IStorrageService
    {

        [OperationContract]
        AnswerResponse Create(List<byte> file, string path);

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

        [OperationContract]
        AnswerResponse Search(string path);
    }



}
