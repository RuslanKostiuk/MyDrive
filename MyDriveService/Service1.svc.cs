using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyDriveService
{
    
    public class AccessService : IAccessService
    {
        MyDriveDB db = new MyDriveDB();
        public AnswerCode UserAuth(string login, string password)
        {
            if (
                 db.Users.Where(user => user.Login == login && user.Password == password).Count() > 0
              ) return AnswerCode.Complete;

            return AnswerCode.Failed;
        }

        public AnswerCode UserRegistration(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return AnswerCode.Complete;
            }
            catch
            {
                return AnswerCode.Failed;
            }
        }
    }
}
