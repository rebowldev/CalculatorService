using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client
{
	public class ServiceClient : IServiceClient
	{
		private readonly HttpClient _httpClient;

		// TODO: Read from app settings
		public string TrackerHeaderKey => "X-Evi-Tracking-Id";

		public ServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.BaseAddress = new Uri(configuration.GetValue<string>("API:BaseAddress"));
		}

		public async Task<ServiceResponse<string>> RequestCalculation<T>(string requestUri, T request, string trackerId) where T : class
		{
			try
			{
				HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
				var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
				if (!string.IsNullOrEmpty(trackerId))
					requestContent.Headers.Add(TrackerHeaderKey, trackerId);
				httpRequest.Content = requestContent;

				var response = await _httpClient.SendAsync(httpRequest);

				string responseContent = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					return ServiceResponse<string>.Sucess(responseContent);
				}
				else
				{
					return ServiceResponse<string>.Error(responseContent);
				}
			}
			catch (Exception ex)
			{
				return ServiceResponse<string>.Error(ex.Message);
			}
		}
	}
}
