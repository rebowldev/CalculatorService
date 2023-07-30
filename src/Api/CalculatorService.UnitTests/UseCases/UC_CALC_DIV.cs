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
	public class UC_CALC_DIV
	{
		[Theory]
		[InlineData(11, -2)]
		[InlineData(int.MaxValue, int.MaxValue)]
		[InlineData(int.MinValue, int.MinValue)]
		[InlineData(int.MaxValue, int.MinValue)]
		public void Success(int dividend, int divisor)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new DivRequest
			{
				Dividend = dividend,
				Divisor = divisor
			};

			// Act
			var result = controller.Div(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<DivResponse>();

			((DivResponse)objectResult.Value).Quotient.Should().Be(dividend / divisor);
			((DivResponse)objectResult.Value).Remainder.Should().Be(dividend % divisor);
		}

		[Fact]
		public void Fail_DivisonByZero()
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new DivRequest
			{
				Dividend = 1,
				Divisor = 0
			};

			// Act
			var result = controller.Div(request);

			// Assert
			result.Should().BeOfType<BadRequestObjectResult>();

			var objectResult = (BadRequestObjectResult)result;
			objectResult.Should().NotBeNull();

			var errorResponse = (ErrorResponse)objectResult.Value;
			errorResponse.Should().NotBeNull();
			errorResponse.ErrorStatus.Should().Be(400);
			errorResponse.ErrorCode.Should().Be("BadRequest");
			errorResponse.ErrorMessage.Should().Be("Unable to divide by zero");
		}

		[Fact]
		public void Fail_Exception()
		{
			// Arrange
			string exceptionMessage = "Unexpected error";

			var calculatorMock = new Mock<ICalculator>();
			calculatorMock.Setup(x => x.Div(It.IsAny<int>(), It.IsAny<int>())).Throws(new ApplicationException(exceptionMessage));
			var controller = new CalculatorController(calculatorMock.Object);
			var request = new DivRequest
			{
				Dividend = 0,
				Divisor = 1
			};

			// Act
			var result = controller.Div(request);

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
