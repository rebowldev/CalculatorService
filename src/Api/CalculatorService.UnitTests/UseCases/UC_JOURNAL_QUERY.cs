using CalculatorService.Api.Controllers;
using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using CalculatorService.Tracker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.UnitTests.UseCases
{
	public class UC_JOURNAL_QUERY
	{
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
			var controller = new JournalController(tracker);

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
	}
}
