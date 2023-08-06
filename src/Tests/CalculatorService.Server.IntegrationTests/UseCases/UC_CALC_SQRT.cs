using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Server.IntegrationTests.UseCases
{
	public class UC_CALC_SQRT : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private const string URI = "/calculator/sqrt";

		public UC_CALC_SQRT(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-546846468)]
		[InlineData(321694986)]
		public async Task Success(double number)
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new SqrtRequest
			{
				Number = number
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = JsonSerializer.Deserialize<SqrtResponse>(content);
			result.Should().NotBeNull();
			result?.Square.Should().Be(Math.Sqrt(Math.Abs(number)));
		}
	}
}
