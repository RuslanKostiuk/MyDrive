using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.ServiceModel;
using System.Configuration;

namespace MyDriveService
{
    public class CustomUserNamePasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDriveCS"].ConnectionString;
            MyDriveDB db = new MyDriveDB(connStr);
            if (db.Users.Where(user => user.Login == userName && user.Password == password).Count() < 1)
            {
                throw new FaultException("Unknown Username or Incorrect Password");
            }
        }
    }
}