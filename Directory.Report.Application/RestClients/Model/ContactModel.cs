using System.Collections.Generic;

namespace Directory.Report.Application.RestClients.Model
{
    public class ContactModel
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