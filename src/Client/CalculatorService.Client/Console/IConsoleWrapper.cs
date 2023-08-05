namespace CalculatorService.Client
{
	public interface IConsoleWrapper
	{
		string ReadLine();

		void Print(string message = "");

		void PrintLine(string message = "");

		void PrintHighlight(string message);

		void PrintSuccess(string message);

		void PrintError(string message);
	}
}
