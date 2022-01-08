using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Directory.Contact.Application.Handlers.Command;
using Directory.Contact.Application.Handlers.Query;
using Directory.Contact.Domain.Entities;
using MicroServices.Infrastructure.Repository;
using MicroServices.Test.Mock;
using Moq;
using Xunit;

namespace Directory.Contact.Test.Handler
{
    public class HandlersTest : RepositoryMock
    {
        Mock<IRepository<Domain.Entities.Contact>> mockRepository = new Mock<IRepository<Domain.Entities.Contact>>();
        List<Domain.Entities.Contact> contacts = new List<Domain.Entities.Contact>();
        
        public HandlersTest()
        {
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "61d7e470e00320030f6535fb",
                Name = "Test1",
                Surname = "Dene1"
            });
            contacts[0].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Email, Value = "a@c.com"});
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "78d7e470e00320030f653590",
                Name = "Test2",
                Surname = "Dene2"
            });
            contacts[1].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Phone, Value = "123"});
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "r8d7e470e00320030f65359y",
                Name = "Test3",
                Surname = "Dene3"
            });
            contacts[2].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Location, Value = "İst"});
            SetupMock<Domain.Entities.Contact>(mockRepository, contacts);
        }

        [Fact]
        public void GetContact()
        {
            var getHandler = new GetContactHandler(mockRepository.Object);
            var mockContact = new Mock<GetContactQuery>
            {
                Object =
                {
                    Id = "61d7e470e00320030f6535fb"
                }
            };
            
            var contact = getHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.NotNull(contact.Model);
            Assert.Equal("Test1", contact.Model.Name);
            Assert.Equal("Dene1", contact.Model.Surname);
        }
        
        [Fact]
        public void ListContact()
        {
            var listHandler = new ListContactHandler(mockRepository.Object);
            var mockContact = new Mock<ListContactQuery>();
            
            var contactList = listHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsType<List<Domain.Entities.Contact>>(contactList.Model);
            Assert.Equal(contacts.Count, contactList.Model.Count);
        }
        
        [Fact]
        public void CreateContact()
        {
            var createHandler = new CreateContactHandler(mockRepository.Object);
            var mockContact = new Mock<CreateContactCommand>
            {
                Object =
                {
                    Name = "Halil",
                    Surname = "Altun", 
                    Company = "X",
                    ContactInfoList = new List<ContactInfo>()
                }
            };
            mockContact.Object.ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Phone, Value = "42333"});
            var contact = createHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(mockContact.Object.Name, contact.Model.Name);
            Assert.NotEmpty(contact.Model.Id);
            Assert.Equal(4, contacts.Count);
        }
        
        [Fact]
        public void DeleteContact()
        {
            var deleteHandler = new DeleteContactHandler(mockRepository.Object);
            var mockContact = new Mock<DeleteContactCommand>
            {
                Object =
                {
                    Id = "r8d7e470e00320030f65359y"
                }
            };
            
            var contact = deleteHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(2, contacts.Count);
        }
        
        [Fact]
        public void AddContactInfo()
        {
            var addInfoHandler = new AddContactInfoHandler(mockRepository.Object);
            var mockContact = new Mock<AddContactInfoCommand>
            {
                Object =
                {
                    ContactId = "61d7e470e00320030f6535fb",
                    ContactInfo = new ContactInfo() { InfoType = EInfoType.Location, Value = "Paris"}
                }
            };
            
            var contact = addInfoHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(2, contact.Model.Count);
        }
        
        [Fact]
        public void RemoveContactInfo()
        {
            var removeInfoHandler = new RemoveContactInfoHandler(mockRepository.Object);
            var mockContact = new Mock<RemoveContactInfoCommand>
            {
                Object =
                {
                    ContactId = "78d7e470e00320030f653590",
                    ContactInfo = new ContactInfo() { InfoType = EInfoType.Phone, Value = "123" }
                }
            };
            
            var contact = removeInfoHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Empty(contact.Model);
        }
    }
}