using CalculatorService.Interfaces.Application;

namespace CalculatorService.Application
{
	public class Calculator : ICalculator
	{
		public double? Add(params double[] addends)
		{
			if (addends?.Length >= 2)
				return addends.Sum();
			else
				return null;
		}

		public double Sub(double minuend, double subtrahend)
		{
			return minuend - subtrahend;
		}

		public double? Mult(params double[] factors)
		{
			if (factors?.Length >= 2)
				return factors.Aggregate(1, (double a, double b) => a * b);
			else
				return null;
		}

		public (int, int) Div(int dividend, int divisor)
		{
			if (dividend == 0)
				throw new DivideByZeroException();

			int quotient = dividend / divisor;
			int remainder = dividend % divisor;

			return (quotient, remainder);
		}

		public double Sqrt(double number)
		{
			return Math.Sqrt(Math.Abs(number));
		}
	}
}
