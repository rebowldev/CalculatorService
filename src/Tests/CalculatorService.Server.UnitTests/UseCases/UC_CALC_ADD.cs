using CalculatorService.Application;
using CalculatorService.Interfaces.Application;
using CalculatorService.Model.DTO;
using CalculatorService.Server;
using CalculatorService.Server.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CalculatorService.UnitTests.UseCases
{
	public class UC_CALC_ADD
    {
		private readonly ILoggerFactory _loggerFactory = new NullLoggerFactory();

		[Theory]
        [InlineData(0, 0)]
		[InlineData(-123.96632, 5.9)]
		[InlineData(double.MinValue, double.MinValue)]
		[InlineData(double.MinValue, double.MaxValue)]
		public void Success_TwoAddends(double addend1, double addend2)
        {
            // Arrange
            var controller = new CalculatorController(new Calculator(), _loggerFactory);
            var request = new AddRequest
            {
                Addends = new double[] { addend1, addend2 }
            };

            // Act
            var result = controller.Add(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<AddResponse>();
            ((AddResponse)objectResult.Value).Sum.Should().Be(addend1 + addend2);
		}

		[Theory]
		[InlineData(new double[] { 0, 0, 0})]
		[InlineData(new double[] { 644.681, -1654.8, 8135 })]
		[InlineData(new double[] { double.MinValue, double.MaxValue, 166.8979 })]
		[InlineData(new double[] { double.MinValue, double.MaxValue, 1654, 8865498.465, 65164.88962 })]
		public void Success_MoreThanTwoAddends(double[] addends)
		{
			// Arrange
			var controller = new CalculatorController(new Calculator(), _loggerFactory);
			var request = new AddRequest
			{
				Addends = addends
			};

			// Act
			var result = controller.Add(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();

			var objectResult = (OkObjectResult)result;
			objectResult.Should().NotBeNull();
			objectResult.Value.Should().NotBeNull();
			objectResult.Value.Should().BeOfType<AddResponse>();
			((AddResponse)objectResult.Value).Sum.Should().Be(addends.Sum());
		}

		[Fact]
		public void Fail_Exception()
		{
			// Arrange
			string exceptionMessage = "Unexpected error";

			var calculatorMock = new Mock<ICalculator>();
			calculatorMock.Setup(x => x.Add(It.IsAny<double[]>())).Throws(new ApplicationException(exceptionMessage));
			var controller = new CalculatorController(calculatorMock.Object, _loggerFactory);
			var request = new AddRequest
			{
				Addends = new double[] { 0, 0 }
			};

			// Act
			var result = controller.Add(request);

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