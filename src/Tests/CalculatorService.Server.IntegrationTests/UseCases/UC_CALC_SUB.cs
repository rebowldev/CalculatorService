using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Server.IntegrationTests.UseCases
{
	public class UC_CALC_SUB : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private const string URI = "/calculator/sub";

		public UC_CALC_SUB(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(656516, 98461598)]
		[InlineData(-1684849, -65)]
		[InlineData(224944984, 65198498)]
		public async Task Success(double minuend, double subtrahend)
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new SubRequest
			{
				Minuend = minuend,
				Subtrahend = subtrahend
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = JsonSerializer.Deserialize<SubResponse>(content);
			result.Should().NotBeNull();
			result?.Difference.Should().Be(minuend - subtrahend);
		}
	}
}
