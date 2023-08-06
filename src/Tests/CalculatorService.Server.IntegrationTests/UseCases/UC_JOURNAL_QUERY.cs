using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Server.IntegrationTests.UseCases
{
	public class UC_JOURNAL_QUERY : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private const string URI = "/journal/query";
		private const string TRACKER_HEADR_KEY = "X-Evi-Tracking-Id";

		public UC_JOURNAL_QUERY(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Success()
		{
			// Arrange
			var random = new Random();
			var client = _factory.CreateClient();
			string trackerId = DateTime.UtcNow.Ticks.ToString();
			int trackedOperations = random.Next(1, 10);
			int notTrackedOperations = random.Next(1, 10);

			await RequestSqrtOperations(client, trackedOperations, trackerId);
			await RequestSqrtOperations(client, notTrackedOperations);

			var request = new JournalRequest
			{
				Id = trackerId
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = JsonSerializer.Deserialize<JournalResponse>(content);
			result.Should().NotBeNull();
			result.Operations.Count().Should().Be(trackedOperations);
		}

		private async Task RequestSqrtOperations(HttpClient client, int numOperations, string trackerId = null)
		{
			for (int i = 0; i < numOperations; i++)
			{
				var requestContent = new StringContent(JsonSerializer.Serialize(new { Number = 0 }), Encoding.UTF8, "application/json");

				if (!string.IsNullOrEmpty(trackerId))
					requestContent.Headers.Add(TRACKER_HEADR_KEY, trackerId);

				await client.PostAsync("/calculator/sqrt", requestContent);
			}
		}
	}
}
