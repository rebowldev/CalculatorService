using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CalculatorService.Server.Controllers
{
	[Route("journal")]
	[ApiController]
	public class JournalController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly ITrackerService<OperationInfo> _trackerService;

		public JournalController(ITrackerService<OperationInfo> trackerService, ILoggerFactory loggerFactory)
		{
			_trackerService = trackerService;
			_logger = loggerFactory.CreateLogger<JournalController>();
		}

		[HttpPost("query"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)System.Net.HttpStatusCode.OK, Description = "Operation success", Type = typeof(JournalResponse))]
		[SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)System.Net.HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public async Task<IActionResult> Query([FromBody] JournalRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					List<OperationInfo> result = await _trackerService.GetOperationsByTracker(request.Id);
					var response = new JournalResponse(result.ToArray());
					return Ok(response);
				}
				else
				{
					return BadRequest(ErrorResponse.BadRequest(ModelState));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				// TODO: Should not return exception details to client
				return StatusCode(500, ErrorResponse.InternalServerError(ex.Message));
			}
		}
	}
}
