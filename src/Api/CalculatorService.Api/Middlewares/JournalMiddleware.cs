using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using System.Net;
using System.Text.Json;

namespace CalculatorService.Api.Middlewares
{
	public class JournalMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ITrackerService<OperationInfo> _trackerService;

		public JournalMiddleware(RequestDelegate next, ITrackerService<OperationInfo> trackerService)
		{
			_next = next;
			_trackerService = trackerService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue(_trackerService.HeaderKey, out var trackerId))
			{
				if (context.Request.Path.StartsWithSegments(new PathString("/calculator"))
					&& context.Response.StatusCode == (int)HttpStatusCode.OK)
				{
					(string, string) requestAndResponse = await GetRequestAndResponse(context);

					await SaveOperationInfo(
						trackerId,
						requestAndResponse.Item1,
						requestAndResponse.Item2,
						context.Request.Path);
				}				
			}
			else
			{
				await _next(context);
			}
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
			await _trackerService.SaveOperation(trackerId, operation);
		}

		private async ValueTask<(string, string)> GetRequestAndResponse(HttpContext context)
		{
			context.Request.EnableBuffering();

			using StreamReader requestReader = new StreamReader(context.Request.Body);
			string requestBody = await requestReader.ReadToEndAsync();
			context.Request.Body.Position = 0;

			Stream originalResponseBody = context.Response.Body;

			using Stream newResponseBody = new MemoryStream();
			context.Response.Body = newResponseBody;

			await _next(context);

			newResponseBody.Position = 0;
			using StreamReader responseReader = new StreamReader(context.Response.Body);
			var responseBody = await responseReader.ReadToEndAsync();

			newResponseBody.Position = 0;
			await newResponseBody.CopyToAsync(originalResponseBody);

			return (requestBody, responseBody);
		}
	}
}
