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
	public class UC_CALC_MUL
	{
		[Theory]
		[InlineData(0, 0)]
		[InlineData(-123.96632, 5.9)]
		[InlineData(double.MinValue, 0)]
		[InlineData(0, double.MaxValue)]
		public void Success_TwoFactors(double factor1, double factor2)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new MultRequest
			{
				Factors = new double[] { factor1, factor2 }
			};

			// Act
			var result = controller.Mult(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<MultResponse>();
			
			((MultResponse)objectResult.Value).Product.Should().Be(factor1 * factor2);
		}

		[Theory]
		[InlineData(new double[] { 0, 0, 0 })]
		[InlineData(new double[] { 48965, 3, 2.5, 654 })]
		[InlineData(new double[] { 644.681, -1654.8, 8135 })]
		public void Success_MoreThanTwoFactors(double[] Factors)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new MultRequest
			{
				Factors = Factors
			};

			// Act
			var result = controller.Mult(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<MultResponse>();

			double product = request.Factors.Aggregate(1, (double a, double b) => a * b);
			((MultResponse)objectResult.Value).Product.Should().Be(product);
		}

		[Theory]
		[InlineData(new double[0])]
		[InlineData(new double[] { 1 })]
		public void Fail_LessThanTwoFactors(double[] factors)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator());
			var request = new MultRequest
			{
				Factors = factors
			};

			// Act
			var result = controller.Mult(request);

			// Assert
			result.Should().BeOfType<BadRequestObjectResult>();

			var objectResult = (BadRequestObjectResult)result;
			objectResult.Should().NotBeNull();

			var errorResponse = (ErrorResponse)objectResult.Value;
			errorResponse.Should().NotBeNull();
			errorResponse.ErrorStatus.Should().Be(400);
			errorResponse.ErrorCode.Should().Be("BadRequest");
		}

		[Fact]
		public void Fail_Exception()
		{
			// Arrange
			string exceptionMessage = "Unexpected error";

			var calculatorMock = new Mock<ICalculator>();
			calculatorMock.Setup(x => x.Mult(It.IsAny<double[]>())).Throws(new ApplicationException(exceptionMessage));
			var controller = new CalculatorController(calculatorMock.Object);
			var request = new MultRequest
			{
				Factors = new double[] { 0, 0 }
			};

			// Act
			var result = controller.Mult(request);

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
