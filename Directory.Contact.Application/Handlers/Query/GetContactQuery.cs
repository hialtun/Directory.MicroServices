using System;
using System.Threading;
using System.Threading.Tasks;
using Directory.Contact.Application.DataAccess;
using MediatR;
using MicroServices.Core.Handler;

namespace Directory.Contact.Application.Handlers.Query
{
    public class GetContactQuery : IRequest<GetContactResponse>
    {
        public string Id { get; set; }
    }

    public class GetContactResponse : ResponseBase<Domain.Entities.Contact>
    {

    }

    public class GetContactHandler : IRequestHandler<GetContactQuery, GetContactResponse>
    {
        public GetContactHandler(ContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly ContactRepository _contactRepository;

        public async Task<GetContactResponse> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            GetContactResponse response = new GetContactResponse() { Success = true };
            try
            {
                var contact = await _contactRepository.GetByIdAsync(request.Id);
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