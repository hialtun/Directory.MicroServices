using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Directory.Contact.API.Controllers;
using Directory.Contact.Application.Handlers.Command;
using Directory.Contact.Application.Handlers.Query;
using Directory.Contact.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Directory.Contact.Test.Controller
{
    public class ControllerTest
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        ContactController mockController;
        List<Domain.Entities.Contact> contacts = new List<Domain.Entities.Contact>();
        
        public ControllerTest()
        {
            mockController = new ContactController(mockMediator.Object);
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "61d7e470e00320030f6535fb",
                Name = "Test1",
                Surname = "Dene1",
                Company = "X"
            });
            contacts[0].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Email, Value = "a@c.com"});
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "78d7e470e00320030f653590",
                Name = "Test2",
                Surname = "Dene2",
                Company = "Y"
            });
            contacts[1].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Phone, Value = "123"});
            contacts.Add(new Domain.Entities.Contact()
            {
                Id = "r8d7e470e00320030f65359y",
                Name = "Test3",
                Surname = "Dene3",
                Company = "Z"
            });
            contacts[2].ContactInfoList.Add(new ContactInfo(){InfoType = EInfoType.Location, Value = "İst"});
        }

        [Fact]
        public void Create()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<CreateContactCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CreateContactCommand cmd, CancellationToken token) =>
                {
                    var newContact = new CreateContactResponse()
                    {
                        Success = true,
                        Model = new Domain.Entities.Contact()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = cmd.Name,
                            Surname = cmd.Surname,
                            Company = cmd.Company,
                            ContactInfoList = cmd.ContactInfoList
                        }
                    };
                    return newContact;
                });

            var res = mockController.Create(new CreateContactCommand()
            {
                Name = "Halil",
                Surname = "Altun",
                Company = "X"
            }).GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var contactRes = res?.Value as CreateContactResponse;
            
            Assert.NotNull(contactRes?.Model);
            Assert.NotNull(contactRes?.Model.Id);
            Assert.Equal("Halil",contactRes?.Model.Name);
        }
        
        [Fact]
        public void Delete()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteContactCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeleteContactCommand cmd, CancellationToken token) =>
                {
                    contacts.RemoveAll(c => c.Id == cmd.Id);
                    var deletedContact = new DeleteContactResponse()
                    {
                        Success = true,
                        Model = new Domain.Entities.Contact()
                        {
                            Id = cmd.Id
                        }
                    };
                    return deletedContact;
                });

            var res = mockController.Delete(new DeleteContactCommand()
            {
                Id = "r8d7e470e00320030f65359y"
            }).GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var contactRes = res?.Value as DeleteContactResponse;
            
            Assert.NotNull(contactRes?.Model);
            Assert.NotNull(contactRes?.Model.Id);
            Assert.Equal(2,contacts.Count);
        }
        
        [Fact]
        public void Get()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetContactQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetContactQuery cmd, CancellationToken token) =>
                {
                    var contactModel = contacts.FirstOrDefault(c => c.Id == cmd.Id);
                    var contact = new GetContactResponse()
                    {
                        Success = true,
                        Model = contactModel
                    };
                    return contact;
                });

            var res = mockController.Get("61d7e470e00320030f6535fb")
                .GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var contactRes = res?.Value as GetContactResponse;
            
            Assert.NotNull(contactRes?.Model);
            Assert.Equal("61d7e470e00320030f6535fb",contactRes.Model.Id);
            Assert.Equal("Test1",contactRes.Model.Name);
        }
        
        [Fact]
        public void List()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<ListContactQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ListContactQuery cmd, CancellationToken token) =>
                {
                    var contact = new ListContactResponse()
                    {
                        Success = true,
                        Model = contacts
                    };
                    return contact;
                });

            var res = mockController.List()
                .GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var contactRes = res?.Value as ListContactResponse;

            if (contactRes?.Model == null) return;
            Assert.NotEmpty(contactRes?.Model);
            Assert.Equal(3, contactRes.Model.Count);
        }
    }
}