using CalculatorService.Model.DTO;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace CalculatorService.Client.UnitTests.UseCases
{
	public class UC_CALC_SQRT
	{
		private static readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>();
		private static IConfiguration _configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(_inMemorySettings)
			.Build();

		[Theory]
		[InlineData(0)]
		[InlineData(double.MaxValue)]
		public void SqrtSuccess(double number)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();
			double result = Math.Sqrt(number);

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue($"sqrt {number}");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			_serviceClientMock.Setup(mock => mock.RequestCalculation(It.IsAny<string>(), It.IsAny<SqrtRequest>(), It.IsAny<string>()))
				.Returns(Task.FromResult(
					ServiceResponse<string>.Sucess(
						JsonSerializer.Serialize(new SqrtResponse { Square = result }))
					));

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintSuccess(
				It.Is<string>(x => x.Contains($"Square: {result}"))), Times.Once());

			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Once());
		}
	}
}
