using Directory.Contact.Application.Handlers.Command;
using Directory.Contact.Application.Handlers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Directory.Contact.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateContactCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            var query = new GetContactQuery
            {
                Id = id
            };
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var query = new ListContactQuery();
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(DeleteContactCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpPost("AddContactInfo")]
        public async Task<IActionResult> AddContactInfo(AddContactInfoCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpPost("RemoveContactInfo")]
        public async Task<IActionResult> RemoveContactInfo(RemoveContactInfoCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }
    }
}