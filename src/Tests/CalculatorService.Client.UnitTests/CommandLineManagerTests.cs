using CalculatorService.Model.DTO;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CalculatorService.Client.UnitTests
{
	public class CommandLineManagerTests
	{
		private static readonly Dictionary<string, string> _inMemorySettings = new Dictionary<string, string>();
		private static IConfiguration _configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(_inMemorySettings)
			.Build();

		[Fact]
		public void Help()
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue("help");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintLine(It.Is<string>(x => x.Contains("Generic syntax"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Never());
		}

		[Fact]
		public void CommandNotFoud()
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue("notExistingCommand");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(x => x.ReadLine()).Returns(() => inputs.Dequeue());

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintError(It.Is<string>(x => x == "Command not found")), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Never());
		}

		[Fact]
		public void ServerError()
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue("add 2 2");
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(x => x.ReadLine()).Returns(() => inputs.Dequeue());

			_serviceClientMock.Setup(mock => mock.RequestCalculation(It.IsAny<string>(), It.IsAny<AddRequest>(), It.IsAny<string>()))
				.Returns(Task.FromResult(ServiceResponse<string>.Error("Internal server error")));

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintError(It.Is<string>(x => x.Contains("Internal server error"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Once());
		}

		[Theory]
		[InlineData("add")]
		[InlineData("add 0")]
		[InlineData("sub")]
		[InlineData("sub 0")]
		[InlineData("mult")]
		[InlineData("mult 0")]
		[InlineData("div")]
		[InlineData("div 0")]
		[InlineData("sqrt")]
		[InlineData("journal")]
		public void InsufficientArguments(string input)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue(input);
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintError(It.Is<string>(x => x.Contains("Insufficient arguments"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Never());
		}

		[Theory]
		[InlineData("sub 0 0 0")]
		[InlineData("div 0 0 0")]
		[InlineData("sqrt 0 0")]
		[InlineData("journal x x")]
		public void TooManyArguments(string input)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue(input);
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintError(It.Is<string>(x => x.Contains("Too many arguments"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Never());
		}

		[Theory]
		[InlineData("add 1 a 3")]
		[InlineData("sub 1 x")]
		[InlineData("sub x 1")]
		[InlineData("mult a 1 2 3")]
		[InlineData("mult 1 2 a 3")]
		[InlineData("div 1 x")]
		[InlineData("div x 1")]
		[InlineData("sqrt a")]
		public void InvalidType(string input)
		{
			// Arrange
			var _consoleMock = new Mock<IConsoleWrapper>();
			var _serviceClientMock = new Mock<IServiceClient>();

			Queue<string> inputs = new Queue<string>();
			inputs.Enqueue(input);
			inputs.Enqueue("exit");

			var cmd = new CommandLineManager(_serviceClientMock.Object, _configuration, _consoleMock.Object);
			_consoleMock.Setup(mock => mock.ReadLine()).Returns(() => inputs.Dequeue());

			// Act
			cmd.Run();

			// Assert
			_consoleMock.Verify(mock => mock.PrintError(It.Is<string>(x => x.Contains("Invalid type"))), Times.Once());
			_serviceClientMock.Verify(mock =>
				mock.RequestCalculation(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()),
				Times.Never());
		}
	}
}
