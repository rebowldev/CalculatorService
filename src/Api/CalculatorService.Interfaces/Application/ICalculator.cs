namespace CalculatorService.Interfaces.Application
{
	public interface ICalculator
	{
		/// <summary>
		/// Returns the addition of two or more operands
		/// </summary>
		/// <param name="addends">Array that contains addends to be sum</param>
		/// <returns>If there are two or more addends returns the corresponding sum, returns null otherwise</returns>
		double? Add(params double[] addends);

		/// <summary>
		/// Returns the difference of two numbers
		/// </summary>
		/// <param name="minuend">Minuend</param>
		/// <param name="subtrahend">Subtrahend</param>
		/// <returns>Difference</returns>
		double Sub(double minuend, double subtrahend);

		/// <summary>
		/// Returns the product of two or more operands
		/// </summary>
		/// <param name="factors">Array that contains factors to be multiply</param>
		/// <returns>If there are two or more factors returns the corresponding product, returns null otherwise</returns>
		double? Mult(params double[] factors);

		/// <summary>
		/// Returns the divison of two numbers
		/// </summary>
		/// <param name="dividend">Dividend</param>
		/// <param name="divisor">Divisor</param>
		/// <returns>Quotien and remainder of the divison</returns>
		(int, int) Div(int dividend, int divisor);

		/// <summary>
		/// Returns the square root of a number
		/// </summary>
		/// <param name="number">Input number</param>
		/// <returns>The square root of input number</returns>
		double Sqrt(double number);
	}
}
