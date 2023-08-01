using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CalculatorService.Server.Middlewares
{
	// TODO: Refactor since it does not meet Single Responsability principle
	public class RequestHandlerMiddleware
	{
		private readonly ILogger _logger;

		private readonly RequestDelegate _next;		
		private readonly ITrackerService<OperationInfo> _trackerService;

		private string _requestBody = string.Empty;
		private string _responseBody = string.Empty;

		public RequestHandlerMiddleware(
			RequestDelegate next,
			ILoggerFactory loggerFactory,
			ITrackerService<OperationInfo> trackerService)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<RequestHandlerMiddleware>();
			_trackerService = trackerService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			await ProcessRequestAndResponse(context);

			if (context.Request.Method != "OPTIONS")
				await LogRequest(context.Request.Path, context.Response.StatusCode);

			if (context.Request.Headers.TryGetValue(_trackerService.HeaderKey, out var trackerId))
			{
				if (context.Request.Path.StartsWithSegments(new PathString("/calculator"))
					&& context.Response.StatusCode == (int)HttpStatusCode.OK)
				{
					await SaveOperationInfo(
						trackerId,
						_requestBody,
						_responseBody,
						context.Request.Path);
				}				
			}
		}

		private async ValueTask LogRequest(string path, int statusCode)
		{
			string message = $"{path} | Request: {_requestBody} | Response: {_responseBody}";

			if (statusCode >= 200 & statusCode < 300)
				_logger.LogInformation(message);

			else if (statusCode >= 400 & statusCode < 500)
				_logger.LogWarning(message);

			else
				_logger.LogError(message);
		}

		private async ValueTask SaveOperationInfo(string trackerId, string requestBody, string responseBody, PathString path)
		{
			switch (path.ToString())
			{
				case "/calculator/add":
					await SaveOperation<AddRequest, AddResponse>("Sum", trackerId, requestBody, responseBody);					
					break;

				case "/calculator/sub":
					await SaveOperation<SubRequest, SubResponse>("Sub", trackerId, requestBody, responseBody);
					break;

				case "/calculator/mult":
					await SaveOperation<MultRequest, MultResponse>("Mult", trackerId, requestBody, responseBody);
					break;

				case "/calculator/div":
					await SaveOperation<DivRequest, DivResponse>("Div", trackerId, requestBody, responseBody);
					break;

				case "/calculator/sqrt":
					await SaveOperation<SqrtRequest, SqrtResponse>("Sqrt", trackerId, requestBody, responseBody);
					break;

				default:
					break;
			}
		}

		private async ValueTask SaveOperation<T, S>(string name, string trackerId, string requestBody, string responseBody)
		{
			T request = JsonSerializer.Deserialize<T>(requestBody);
			S response = JsonSerializer.Deserialize<S>(responseBody);

			OperationInfo operation = new OperationInfo
			{
				Operation = name,
				Calculation = $"{request.ToString()} = {response.ToString()}",
				Date = DateTime.UtcNow
			};

			_logger.LogDebug($"Save operation: '{operation.Operation}' [{operation.Calculation}] for trackerId '{trackerId}'");

			await _trackerService.SaveOperation(trackerId, operation);
		}

		private async ValueTask ProcessRequestAndResponse(HttpContext context)
		{
			context.Request.EnableBuffering();

			using StreamReader requestReader = new StreamReader(context.Request.Body);
			_requestBody = (await requestReader.ReadToEndAsync()).Replace("\n", string.Empty);
			context.Request.Body.Position = 0;

			Stream originalResponseBody = context.Response.Body;

			using Stream responseBody = new MemoryStream();
			context.Response.Body = responseBody;
			await _next.Invoke(context);

			responseBody.Seek(0, SeekOrigin.Begin);
			using StreamReader responseReader = new StreamReader(context.Response.Body);
			_responseBody = await responseReader.ReadToEndAsync();

			responseBody.Seek(0, SeekOrigin.Begin);
			await responseBody.CopyToAsync(originalResponseBody);
		}
	}
}
