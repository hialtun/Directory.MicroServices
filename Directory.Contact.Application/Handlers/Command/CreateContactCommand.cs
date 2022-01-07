using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Directory.Contact.Application.DataAccess;
using Directory.Contact.Domain.Entities;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Contact.Application.Handlers.Command
{
    public class CreateContactCommand : IRequest<CreateContactResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactInfo> ContactInfoList { get; set; }
    }

    public class CreateContactResponse : ResponseBase<Domain.Entities.Contact>
    {

    }

    public class CreateContactHandler : IRequestHandler<CreateContactCommand, CreateContactResponse>
    {
        public CreateContactHandler(IRepository<Domain.Entities.Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly IRepository<Domain.Entities.Contact> _contactRepository;

        public async Task<CreateContactResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            CreateContactResponse response = new CreateContactResponse() { Success = true };
            try
            {
                var contact = new Domain.Entities.Contact()
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Company = request.Company,
                    ContactInfoList = request.ContactInfoList
                };
                contact = await _contactRepository.CreateAsync(contact);
                response.Model = contact;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Success = false;
            }
            return response;
        }
    }
}