using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Server.IntegrationTests.UseCases
{
	public class UC_CALC_ADD : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private const string URI = "/calculator/add";

		public UC_CALC_ADD(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData(new double[] { 0, 0, 0 })]
		[InlineData(new double[] { 644.681, -1654.8, 8135 })]
		[InlineData(new double[] { 1, -94615, 166.8979 })]
		[InlineData(new double[] { 4981316.8486, 615615651894984.2, 8865498.465, 65164.88962 })]
		public async Task Success(double[] addends)
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new AddRequest
			{
				Addends = addends
			};
			var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

			// Act
			var response = await client.PostAsync(URI, requestContent);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = JsonSerializer.Deserialize<AddResponse>(content);
			result.Should().NotBeNull();
			result?.Sum.Should().Be(addends.Sum());
		}

		[Theory]
		[InlineData(new double[0])]
		[InlineData(new double[] { 0 })]		
		public async Task BadRequest(double[] addends)
		{
			// Arrange
			var client = _factory.CreateClient();
			var request = new AddRequest
			{
				Addends = addends
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
		}
	}
}
