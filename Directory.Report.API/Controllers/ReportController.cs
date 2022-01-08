using System.Threading.Tasks;
using Directory.Report.Application.Handlers.Command;
using Directory.Report.Application.Handlers.Event;
using Directory.Report.Application.Handlers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Directory.Report.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Demand")]
        public async Task<IActionResult> Demand(ReportDemandEvent @event)
        {
            var result = await _mediator.Send(@event);
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            var query = new GetReportQuery
            {
                Id = id
            };
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var query = new ListReportQuery();
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }
        
    }
}