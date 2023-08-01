using CalculatorService.Server.Controllers;
using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using CalculatorService.Tracker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CalculatorService.UnitTests.UseCases
{
	public class UC_JOURNAL_QUERY
	{
		private Mock<ILoggerFactory> _logger = new Mock<ILoggerFactory>();

		[Fact]
		public async Task QueryOperations()
		{
			// Arrange
			string targetTrackerId = "1";

			ITrackerService<OperationInfo> tracker = new InMemoryTrackerService<OperationInfo>();
			await tracker.SaveOperation("1", new OperationInfo { Operation = "Sum", Calculation = "", Date = DateTime.UtcNow });
			await tracker.SaveOperation("1", new OperationInfo { Operation = "Mul", Calculation = "", Date = DateTime.UtcNow });
			await tracker.SaveOperation("2", new OperationInfo { Operation = "Sqrt", Calculation = "", Date = DateTime.UtcNow });
			await tracker.SaveOperation("2", new OperationInfo { Operation = "Sum", Calculation = "", Date = DateTime.UtcNow });
			await tracker.SaveOperation("3", new OperationInfo { Operation = "Div", Calculation = "", Date = DateTime.UtcNow });
			var controller = new JournalController(tracker, _logger.Object);

			var request = new JournalRequest { Id = targetTrackerId };

			// Act
			var result = await controller.Query(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<JournalResponse>();
			((JournalResponse)objectResult.Value).Operations.Count.Should().Be(2);
		}

		[Fact]
		public async Task SaveOperation()
		{
			// Arrange
			string targetTrackerId = "1";

			ITrackerService<OperationInfo> tracker = new InMemoryTrackerService<OperationInfo>();
			string trackerId = Guid.NewGuid().ToString();
			var operation = new OperationInfo { Operation = "Sum", Calculation = "1 + 1 = 2", Date = DateTime.UtcNow };

			// Act
			await tracker.SaveOperation(trackerId, operation);

			// Assert
			var operations = await tracker.GetOperationsByTracker(trackerId);

			operations.Should().NotBeNull();
			operations.Should().BeOfType<List<OperationInfo>>();
			operations.Count.Should().Be(1);
			operations.First().Operation.Should().Be(operation.Operation);
			operations.First().Calculation.Should().Be(operation.Calculation);
			operations.First().Date.Should().Be(operation.Date);
		}
	}
}
