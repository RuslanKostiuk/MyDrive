using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MyDriveService
{


    [DataContract]
    enum Roles
    {
        [EnumMember]
        trial,

        [EnumMember]
        simple,

        [EnumMember]
        maximum
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        Roles Role { get; set; }
              

    }


    public class MyDriveDB : DbContext
    {
        public MyDriveDB()
        {
        }

        public MyDriveDB(string connection) : base(connection) { }
        public virtual DbSet<User> Users { get; set; }
    }
}