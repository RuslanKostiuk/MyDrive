using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyDriveService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.

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
