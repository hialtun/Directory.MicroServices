using System;
using System.Threading;
using System.Threading.Tasks;
using Directory.Contact.Application.DataAccess;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Contact.Application.Handlers.Command
{
    public class DeleteContactCommand : IRequest<DeleteContactResponse>
    {
        public string Id { get; set; }
    }

    public class DeleteContactResponse : ResponseBase<Domain.Entities.Contact>
    {

    }

    public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, DeleteContactResponse>
    {
        public DeleteContactHandler(IRepository<Domain.Entities.Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }
        private readonly IRepository<Domain.Entities.Contact> _contactRepository;

        public async Task<DeleteContactResponse> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            DeleteContactResponse response = new DeleteContactResponse() { Success = true };
            try
            {
                await _contactRepository.DeleteAsync(request.Id);
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