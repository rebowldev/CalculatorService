using System.Globalization;
using System.Text;

namespace CalculatorService.Client.CalculatorCommands
{
	internal abstract class CalculatorCommand
	{
		public enum Command
		{
			add,
			sub,
			mult,
			div,
			sqrt,
			journal
		}	

		protected abstract bool ValidateArgumentsCount();

		protected abstract bool ValidateArgumentsType();

		protected abstract ValueTask<bool> RequestOperation();

		public abstract string GetCommandHelp();

		protected static string[] Commands => Enum.GetNames(typeof(Command));

		public static readonly string[] NumberDecimalSeparators = new string[] { ",", "." };

		protected string[] _args = new string[0];
		protected string _trackerId = string.Empty;
		protected string _serviceUri = string.Empty;
		protected readonly IServiceClient _client;
		protected StringBuilder _resultBuilder = new StringBuilder();

		public string CommandResult => _resultBuilder.ToString();

		public CalculatorCommand(IServiceClient client, string serviceUri)
		{
			_client = client;
			_serviceUri = serviceUri;			
		}

		public async ValueTask<bool> Run(string[] args)
		{
			_args = args.Skip(1).ToArray();

			ProcessOptionalParamters();

			if (ValidateArgumentsCount() && ValidateArgumentsType())
				return await RequestOperation();

			return false;
		}

		protected virtual void ProcessOptionalParamters()
		{
			List<string> newArgs = new List<string>();
			foreach (string arg in _args)
			{
				if (arg.ToLowerInvariant().StartsWith("-t:"))
				{
					string[] paramParts = arg.Split("-t:");
					_trackerId = paramParts.Length > 1 ? paramParts[1] : string.Empty;
				}
				else
				{
					newArgs.Add(arg);
				}
			}

			_args = newArgs.ToArray();
		}

		public static string GetHelp()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Generic syntax: ");
			stringBuilder.AppendLine($"<{string.Join("|", Commands)}> [options] <operand-1> [operand-2] ... [operand-N]");
			stringBuilder.AppendLine();

			return stringBuilder.ToString();
		}

		public static string GetOptionsHelp()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Options:");
			stringBuilder.AppendLine($"-t:<trackerId>\t\tJournal tracker identifier");

			return stringBuilder.ToString();
		}

		protected static double ToDouble(string value)
		{
			foreach (string separator in NumberDecimalSeparators)
				value = value.Replace(separator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

			if (double.TryParse(value, out double result))
				return result;

			return 0;
		}
	}
}
