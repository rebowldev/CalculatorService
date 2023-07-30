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

		/// <summary>
		/// Returns difference of two numbers
		/// </summary>
		/// <param name="minuend">Minuend</param>
		/// <param name="subtrahend">Subtrahend</param>
		/// <returns>Difference</returns>
		double Sub(double minuend, double subtrahend);

		/// <summary>
		/// Returns product of two or more operands
		/// </summary>
		/// <param name="addends">Array that contains factors to be multiply</param>
		/// <returns>If there are two or more factors returns the corresponding product, returns null otherwise</returns>
		double? Mult(params double[] addends);
	}
}
