using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.Api.Controllers
{
	[Route("journal")]
	[ApiController]
	public class JournalController : ControllerBase
	{
		private readonly ITrackerService<OperationInfo> _trackerService;

		public JournalController(ITrackerService<OperationInfo> trackerService)
		{
			_trackerService = trackerService;
		}

		[HttpPost("query"), FormatFilter]
		public async Task<IActionResult> Query([FromBody] JournalRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					List<OperationInfo> result = await _trackerService.GetOperationsByTracker(request.Id);
					var response = new JournalResponse(result);
					return Ok(response);
				}
				else
				{
					return BadRequest(ErrorResponse.BadRequest(ModelState));
				}
			}
			catch (Exception ex)
			{
				// TODO: Should not return exception details to client
				return StatusCode(500, ErrorResponse.InternalServerError(ex.Message));
			}
		}
	}
}
