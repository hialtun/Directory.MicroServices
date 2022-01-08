using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Directory.Contact.Application.DataAccess;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Contact.Application.Handlers.Query
{
    public class ListContactQuery : IRequest<ListContactResponse>
    {

    }

    public class ListContactResponse : ResponseBase<List<Domain.Entities.Contact>>
    {

    }

    public class ListContactHandler : IRequestHandler<ListContactQuery, ListContactResponse>
    {
        public ListContactHandler(IRepository<Domain.Entities.Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly IRepository<Domain.Entities.Contact> _contactRepository;

        public async Task<ListContactResponse> Handle(ListContactQuery request, CancellationToken cancellationToken)
        {
            var response = new ListContactResponse() { Success = true };
            try
            {
                var contactQuery =  _contactRepository.Get();
                response.Model = contactQuery.ToList();
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