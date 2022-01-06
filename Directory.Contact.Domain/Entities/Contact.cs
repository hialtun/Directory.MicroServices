using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using MicroServices.Core.Entity;
using MongoDB.Bson;

namespace Directory.Contact.Domain.Entities
{
    public class Contact : DocumentEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactInfo> ContactInfoList { get; set; }
    }

    public class ContactInfo
    {
        public EInfoType InfoType { get; set; }
        public string Value { get; set; }
    }
    
    public enum EInfoType
    {
        Phone,
        Email,
        Location
    }
}