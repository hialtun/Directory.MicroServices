using MicroServices.Infrastructure.Repository;
using MicroServices.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace Directory.Contact.Application.DataAccess
{
    public class ContactRepository : GenericRepository<Domain.Entities.Contact>
    {
        public ContactRepository(IOptions<DatabaseSettings> options) : base(options)
        {

        }
    }
}