using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json.Serialization;

namespace CalculatorService.Server
{
	public class ErrorResponse
	{
		[JsonPropertyName("ErrorCode")]
		public string ErrorCode { get; set; } = string.Empty;

		[JsonPropertyName("ErrorStatus")]
		public int ErrorStatus { get; set; }

		[JsonPropertyName("ErrorMessage")]
		public string ErrorMessage { get; set; } = string.Empty;

		public static ErrorResponse BadRequest(ModelStateDictionary modelState)
		{
			IEnumerable<string> errors = modelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage);

			return BadRequest(string.Join(" | ", errors));
		}

		public static ErrorResponse BadRequest(string message)
		{
			return new ErrorResponse
			{
				ErrorCode = HttpStatusCode.BadRequest.ToString(),
				ErrorStatus = (int)HttpStatusCode.BadRequest,
				ErrorMessage = message
			};
		}

		public static ErrorResponse InternalServerError()
		{
			return new ErrorResponse
			{
				ErrorCode = HttpStatusCode.InternalServerError.ToString(),
				ErrorStatus = (int)HttpStatusCode.InternalServerError,
				ErrorMessage = "An unexpected error condition was triggered wich made impossible to fulfill the request. Please try again."
			};
		}
	}
}
