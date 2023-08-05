using CalculatorService.Model.DTO;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace CalculatorService.Client.UnitTests.UseCases
{
	public class UC_CALC_DIV
	{
		private static readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>();
		private static IConfiguration _configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(_inMemorySettings)
			.Build();

		[Theory]
		[InlineData(1, 1)]
		[InlineData(11, 4)]
		[InlineData(int.MaxValue, -5987)]
		public void DivSuccess(int dividend, int divisor)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();
			int quotient = dividend / divisor;
			int remainder = dividend % divisor;

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue($"div {dividend} {divisor}");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			_serviceClientMock.Setup(mock => mock.RequestCalculation(It.IsAny<string>(), It.IsAny<DivRequest>(), It.IsAny<string>()))
				.Returns(Task.FromResult(
					ServiceResponse<string>.Sucess(
						JsonSerializer.Serialize(new DivResponse { Quotient = quotient, Remainder = remainder }))
					));

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintSuccess(
				It.Is<string>(x => x.Contains($"Quotient: {quotient}") && x.Contains($"Remainder: {remainder}"))), Times.Once());

			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Once());
		}
	}
}
