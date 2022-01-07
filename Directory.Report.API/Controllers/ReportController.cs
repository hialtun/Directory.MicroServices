using System.Threading.Tasks;
using Directory.Report.Application.Handlers.Event;
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
        private readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("Demand")]
        public async Task<IActionResult> Demand(ReportDemandEvent @event)
        {
            var result = await _mediator.Send(@event);
            return result != null ? Ok(result) : NotFound();
        }
    }
}