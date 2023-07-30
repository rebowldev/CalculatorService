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
	public class UC_CALC_SUB
	{
		[Theory]
		[InlineData(0, 0)]
		[InlineData(3, -7)]
		[InlineData(14, 38)]
		[InlineData(-15691, 9874)]
		[InlineData(-8461, -2)]
		public void Success(double minuend, double subtrahend)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new SubRequest
			{
				Minuend = minuend,
				Subtrahend = subtrahend
			};

			// Act
			var result = controller.Sub(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<SubResponse>();
			((SubResponse)objectResult.Value).Difference.Should().Be(minuend - subtrahend);
		}

		[Fact]
		public void Fail_Exception()
		{
			// Arrange
			string exceptionMessage = "Unexpected error";

			var calculatorMock = new Mock<ICalculator>();
			calculatorMock.Setup(x => x.Sub(It.IsAny<double>(), It.IsAny<double>())).Throws(new ApplicationException(exceptionMessage));
			var controller = new CalculatorController(calculatorMock.Object);
			var request = new SubRequest
			{
				Minuend = 0,
				Subtrahend = 0
			};

			// Act
			var result = controller.Sub(request);

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
