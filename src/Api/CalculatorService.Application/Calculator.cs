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
	}
}
