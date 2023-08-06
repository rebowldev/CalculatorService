using CalculatorService.Model.DTO;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace CalculatorService.Client.UnitTests.UseCases
{
	public class UC_CALC_MUL
	{
		private static readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>();
		private static IConfiguration _configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(_inMemorySettings)
			.Build();

		[Theory]
		[InlineData(new double[] { 0, 0 })]
		[InlineData(new double[] { -8465, 9, 8456, 33 })]
		public void MultSuccess(double[] factors)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();
			double result = factors.Aggregate(1, (double a, double b) => a * b);

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue("mult " + string.Join(" ", factors));
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			_serviceClientMock.Setup(mock => mock.RequestCalculation(It.IsAny<string>(), It.IsAny<MultRequest>(), It.IsAny<string>()))
				.Returns(Task.FromResult(
					ServiceResponse<string>.Sucess(JsonSerializer.Serialize(new MultResponse { Product = result }))
					));

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintSuccess(It.Is<string>(x => x.Contains($"Product: {result}"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Once());
		}
	}
}
