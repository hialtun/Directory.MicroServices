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
    public class RemoveContactInfoCommand : IRequest<RemoveContactInfoResponse>
    {
        public string ContactId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    public class RemoveContactInfoResponse : ResponseBase<List<ContactInfo>>
    {

    }

    public class RemoveContactInfoHandler : IRequestHandler<RemoveContactInfoCommand, RemoveContactInfoResponse>
    {
        public RemoveContactInfoHandler(IRepository<Domain.Entities.Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly IRepository<Domain.Entities.Contact> _contactRepository;

        public async Task<RemoveContactInfoResponse> Handle(RemoveContactInfoCommand request, CancellationToken cancellationToken)
        {
            RemoveContactInfoResponse response = new RemoveContactInfoResponse() { Success = true };
            try
            {
                var contact = await  _contactRepository.GetByIdAsync(request.ContactId);
                contact.ContactInfoList.RemoveAll(c => c.InfoType == request.ContactInfo.InfoType
                && c.InfoType == request.ContactInfo.InfoType);
                await _contactRepository.UpdateAsync(contact.Id, contact);
                response.Model = contact.ContactInfoList;
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