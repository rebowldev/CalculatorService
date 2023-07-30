using CalculatorService.Api;
using CalculatorService.Api.Controllers;
using CalculatorService.Application;
using CalculatorService.Interfaces.Application;
using CalculatorService.Model.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CalculatorService.UnitTests.UseCases
{
	public class UC_CALC_SQRT
	{
		[Theory]
		[InlineData(0)]
		[InlineData(16)]
		[InlineData(double.MinValue)]
		[InlineData(double.MaxValue)]
		public void Success(double number)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new SqrtRequest
			{
				Number = number
			};

			// Act
			var result = controller.Sqrt(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<SqrtResponse>();
			((SqrtResponse)objectResult.Value).Square.Should().Be(Math.Sqrt(Math.Abs(number)));
		}

		[Fact]
		public void Fail_Exception()
		{
			// Arrange
			string exceptionMessage = "Unexpected error";

			var calculatorMock = new Mock<ICalculator>();
			calculatorMock.Setup(x => x.Sqrt(It.IsAny<double>())).Throws(new ApplicationException(exceptionMessage));
			var controller = new CalculatorController(calculatorMock.Object);
			var request = new SqrtRequest
			{
				Number = 0
			};

			// Act
			var result = controller.Sqrt(request);

			// Assert
			result.Should().BeOfType<ObjectResult>();

			var objectResult = (ObjectResult)result;
			objectResult.Should().NotBeNull();

			var errorResponse = (ErrorResponse)objectResult.Value;
			errorResponse.Should().NotBeNull();
			errorResponse.ErrorStatus.Should().Be(500);
			errorResponse.ErrorCode.Should().Be("InternalServerError");
			errorResponse.ErrorMessage.Should().Be(exceptionMessage);
		}
	}
}
