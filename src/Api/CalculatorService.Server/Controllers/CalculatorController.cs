using CalculatorService.Interfaces.Application;
using CalculatorService.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CalculatorService.Server.Controllers
{
	[Route("calculator")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly ICalculator _calculator;

		public CalculatorController(ICalculator calculator, ILoggerFactory loggerFactory)
		{
			_calculator = calculator;
			_logger = loggerFactory.CreateLogger<CalculatorController>();
		}

		[HttpPost("add"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)HttpStatusCode.OK, Description = "Operation success", Type = typeof(AddResponse))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public IActionResult Add([FromBody] AddRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					double? result = _calculator.Add(request.Addends);

					var response = new AddResponse { Sum = result.Value };
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.InternalServerError());
			}
		}

		[HttpPost("sub"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)HttpStatusCode.OK, Description = "Operation success", Type = typeof(SubResponse))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public IActionResult Sub([FromBody] SubRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					double? result = _calculator.Sub(request.Minuend, request.Subtrahend);

					var response = new SubResponse { Difference = result.Value };
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.InternalServerError());
			}
		}

		[HttpPost("mult"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)HttpStatusCode.OK, Description = "Operation success", Type = typeof(MultResponse))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public IActionResult Mult([FromBody] MultRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					double? result = _calculator.Mult(request.Factors);

					if (result.HasValue)
					{
						var response = new MultResponse { Product = result.Value };
						return Ok(response);
					}
					else
					{
						return BadRequest(ErrorResponse.BadRequest("Insufficient factors"));
					}
				}
				else
				{
					return BadRequest(ErrorResponse.BadRequest(ModelState));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.InternalServerError());
			}
		}

		[HttpPost("div"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)HttpStatusCode.OK, Description = "Operation success", Type = typeof(DivResponse))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public IActionResult Div([FromBody] DivRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (request.Divisor != 0)
					{
						(int, int) result = _calculator.Div(request.Dividend, request.Divisor);

						var response = new DivResponse
						{
							Quotient = result.Item1,
							Remainder = result.Item2
						};
						return Ok(response);
					}
					else
					{
						return BadRequest(ErrorResponse.BadRequest("Divison by zero attempt"));
					}					
				}
				else
				{
					return BadRequest(ErrorResponse.BadRequest(ModelState));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.InternalServerError());
			}
		}

		[HttpPost("sqrt"), FormatFilter]
		[Consumes("application/json", "application/xml")]
		[Produces("application/json", "application/xml")]
		[SwaggerResponse((int)HttpStatusCode.OK, Description = "Operation success", Type = typeof(SqrtResponse))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "BadRequest", Type = typeof(ErrorResponse))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error", Type = typeof(ErrorResponse))]
		public IActionResult Sqrt([FromBody] SqrtRequest request, [FromQuery] string? format = "json")
		{
			try
			{
				if (ModelState.IsValid)
				{
					double? result = _calculator.Sqrt(request.Number);

					var response = new SqrtResponse { Square = result.Value };
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
				return StatusCode((int)HttpStatusCode.InternalServerError, ErrorResponse.InternalServerError());
			}
		}
	}
}
