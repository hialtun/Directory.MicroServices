﻿using Directory.Contact.Application.Handlers.Command;
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
        private readonly ILogger<ContactController> _logger;
        private readonly IMediator _mediator;

        public ContactController(ILogger<ContactController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateContactCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get(GetContactQuery query)
        {
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("list")]
        public async Task<IActionResult> List(ListContactQuery query)
        {
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteContactCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? Ok(result) : NotFound();
        }
    }
}