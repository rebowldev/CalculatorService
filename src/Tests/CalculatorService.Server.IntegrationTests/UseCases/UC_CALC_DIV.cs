using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Server.IntegrationTests.UseCases
{
	public class UC_CALC_DIV : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private const string URI = "/calculator/div";

		public UC_CALC_DIV(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData(0, 1)]
		[InlineData(656516, 98461598)]
		[InlineData(-1684849, -65)]
		[InlineData(224944984, 65198498)]
		public async Task Success(int dividend, int divisor)
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new DivRequest
			{
				Dividend = dividend,
				Divisor = divisor
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = JsonSerializer.Deserialize<DivResponse>(content);
			result.Should().NotBeNull();
			result?.Quotient.Should().Be(dividend / divisor);
			result?.Remainder.Should().Be(dividend % divisor);
		}

		[Fact]
		public async Task DivisonByZeroBadRequest()
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new DivRequest
			{
				Dividend = 1
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var result = JsonSerializer.Deserialize<ErrorResponse>(content);
			result.Should().NotBeNull();
			result?.ErrorCode.Should().Be("BadRequest");
			result?.ErrorStatus.Should().Be((int)HttpStatusCode.BadRequest);
			result?.ErrorMessage.Should().Be("Divison by zero attempt");
		}
	}
}
