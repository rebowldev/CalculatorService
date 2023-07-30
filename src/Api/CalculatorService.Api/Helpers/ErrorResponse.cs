using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace CalculatorService.Api
{
	public class ErrorResponse
	{
		public string ErrorCode { get; set; } = string.Empty;

		public int ErrorStatus { get; set; }

		public string ErrorMessage { get; set; } = string.Empty;

		public static ErrorResponse BadRequest(ModelStateDictionary modelState)
		{
			string errors = string.Join(" | ", modelState.Values);
			return BadRequest(errors);
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

		public static ErrorResponse InternalServerError(string message)
		{
			return new ErrorResponse
			{
				ErrorCode = HttpStatusCode.InternalServerError.ToString(),
				ErrorStatus = (int)HttpStatusCode.InternalServerError,
				ErrorMessage = message
			};
		}
	}
}
