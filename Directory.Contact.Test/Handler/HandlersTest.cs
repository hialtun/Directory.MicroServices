using System.Collections.Generic;
using System.Threading;
using Directory.Contact.Application.Handlers.Command;
using Directory.Contact.Application.Handlers.Query;
using Directory.Contact.Test.Mock;
using MicroServices.Infrastructure.Repository;
using Moq;
using Xunit;

namespace Directory.Contact.Test.Handler
{
    public class HandlersTest : GenericRepositoryMock
    {
        Mock<IRepository<Domain.Entities.Contact>> mockRepository = new Mock<IRepository<Domain.Entities.Contact>>();
        List<Domain.Entities.Contact> contacts = new List<Domain.Entities.Contact>();
        
        public HandlersTest()
        {
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "61d7e470e00320030f6535fb",
                Name = "Test",
                Surname = "Dene"
            });
            SetupMock<Domain.Entities.Contact>(mockRepository, contacts);
        }
        
        [Fact]
        public void CreateContact()
        {
            var createHandler = new CreateContactHandler(mockRepository.Object);
            var mockContact = new Mock<CreateContactCommand>
            {
                Object =
                {
                    Name = "Halil",Surname = "Altun"
                }
            };
            
            var contact = createHandler.Handle(mockContact.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(mockContact.Object.Name, contact.Model.Name);
            Assert.NotEmpty(contact.Model.Id);
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
            Assert.Equal("Test", contact.Model.Name);
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
    }
}