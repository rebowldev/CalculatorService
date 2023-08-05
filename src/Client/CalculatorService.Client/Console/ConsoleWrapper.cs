namespace CalculatorService.Client
{
	public class ConsoleWrapper : IConsoleWrapper
	{
		public string ReadLine()
		{
			return Console.ReadLine() ?? string.Empty;
		}

		public void Print(string message = "")
		{
			Console.Write(message);
		}

		public void PrintLine(string message)
		{
			Console.WriteLine(message);
		}

		public void PrintHighlight(string message)
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		public void PrintSuccess(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		public void PrintError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}
