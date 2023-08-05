using CalculatorService.Model.DTO;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace CalculatorService.Client.UnitTests.UseCases
{
	public class UC_JOURNAL_QUERY
	{
		private static readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>();
		private static IConfiguration _configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(_inMemorySettings)
			.Build();

		[Fact]
		public void JournalSuccess()
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue("journal xxx");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			_serviceClientMock.Setup(mock => mock.RequestCalculation(It.IsAny<string>(), It.IsAny<JournalRequest>(), It.IsAny<string>()))
				.Returns(Task.FromResult(
					ServiceResponse<string>.Sucess(
						JsonSerializer.Serialize(new JournalResponse(new OperationInfo[]
							{
								new OperationInfo { Operation = "Test operation", Calculation = "", Date = DateTime.Now }
							}))
						)));

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintSuccess(
				It.Is<string>(x => x.Contains($"Test operation"))), Times.Once());

			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Once());
		}
	}
}
