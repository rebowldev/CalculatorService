using CalculatorService.Interfaces.Application;
using CalculatorService.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.Api.Controllers
{
	[Route("calculator")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private readonly ICalculator _calculator;

		public CalculatorController(ICalculator calculator)
		{
			_calculator = calculator;
		}

		[HttpPost("add"), FormatFilter]
		public IActionResult Add([FromBody] AddRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					double? result = _calculator.Add(request.Addends);

					if (result.HasValue)
					{
						var response = new AddResponse { Sum = result.Value };
						return Ok(response);
					}
					else
					{
						return BadRequest(ErrorResponse.BadRequest("Insufficient addends"));
					}
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
