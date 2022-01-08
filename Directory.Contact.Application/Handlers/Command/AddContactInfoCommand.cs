﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Directory.Contact.Domain.Entities;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Contact.Application.Handlers.Command
{
    public class AddContactInfoCommand : IRequest<AddContactInfoResponse>
    {
        public string ContactId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    public class AddContactInfoResponse : ResponseBase<List<ContactInfo>>
    {

    }

    public class AddContactInfoHandler : IRequestHandler<AddContactInfoCommand, AddContactInfoResponse>
    {
        public AddContactInfoHandler(IRepository<Domain.Entities.Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly IRepository<Domain.Entities.Contact> _contactRepository;

        public async Task<AddContactInfoResponse> Handle(AddContactInfoCommand request, CancellationToken cancellationToken)
        {
            AddContactInfoResponse response = new AddContactInfoResponse() { Success = true };
            try
            {
                var contact = await  _contactRepository.GetByIdAsync(request.ContactId);
                contact.ContactInfoList ??= new List<ContactInfo>();
                contact.ContactInfoList.Add(
                    new ContactInfo()
                    {
                        InfoType = request.ContactInfo.InfoType,
                        Value = request.ContactInfo.Value
                    }
                    );
                await _contactRepository.UpdateAsync(contact);
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