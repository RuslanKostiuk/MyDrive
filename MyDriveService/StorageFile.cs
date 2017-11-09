using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MyDriveService
{
    [DataContract]
    public class StorageFile
    {
 
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte[] Bytes;
    }

    
}