namespace CalculatorService.Interfaces.Application
{
	public interface ICalculator
	{
		/// <summary>
		/// Returns addition of two or more operands
		/// </summary>
		/// <param name="addends">Array that contains addends to be sum</param>
		/// <returns>If there are two or more addends returns the corresponding sum, returns null otherwise</returns>
		double? Add(params double[] addends);

		double Sub(double minuend, double subtrahend);
	}
}
