using CalculatorService.Client.CalculatorCommands;
using Microsoft.Extensions.Configuration;

namespace CalculatorService.Client
{
	public class CommandLineManager
	{
		private readonly IServiceClient _client;
		private readonly IConfiguration _configuration;
		private readonly IConsoleWrapper _console;

		private const string EXIT_CMD = "exit";
		private const string HELP_CMD = "help";

		public CommandLineManager(
			IServiceClient client,
			IConfiguration configuration,
			IConsoleWrapper consoleWrapper)
		{
			_client = client;
			_configuration = configuration;
			_console = consoleWrapper;
		}

		public void Run()
		{
			_console.PrintLine("------CALCULATOR SERVICE CLIENT ------");
			_console.PrintLine("Enter \"help\" to see the documentation");
			_console.PrintHighlight($"Allowed separators for decimal numbers: \"{string.Join("\" \"", CalculatorCommand.NumberDecimalSeparators)}\"");
			_console.PrintLine();

			string input;
			do
			{
				input = Prompt() ?? string.Empty;

				if (!string.IsNullOrEmpty(input))
					ProcessInput(input).AsTask().Wait();
			}
			while (input?.ToString().ToLower() != EXIT_CMD);
			
		}

		private string? Prompt()
		{
			_console.PrintLine();
			_console.Print("calc> ");
			return _console.ReadLine();
		}

		private async ValueTask ProcessInput(string input)
		{
			if (input.ToLowerInvariant() == EXIT_CMD)
				return;

			if (input.ToLowerInvariant() == HELP_CMD)
			{
				_console.PrintLine(CalculatorCommand.GetHelp());
				return;
			}

			string[] args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			CalculatorCommand calculatorCommand = GetCalculatorCommand(args[0]);

			if (calculatorCommand != null)
			{
				var result = await calculatorCommand.Run(args);

				if (result)
				{
					_console.PrintSuccess(calculatorCommand.CommandResult);
				}					
				else
				{
					_console.PrintError(calculatorCommand.CommandResult);
					_console.PrintLine(calculatorCommand.GetCommandHelp());
				}
			}
			else
			{
				_console.PrintError("Command not found");
				_console.PrintLine(CalculatorCommand.GetHelp());
			}
		}

		private CalculatorCommand? GetCalculatorCommand(string inputCommand)
		{
			CalculatorCommand? result = null;

			if (Enum.TryParse<CalculatorCommand.Command>(inputCommand.ToLowerInvariant(), out var command))
			{
				switch (command)
				{
					case CalculatorCommand.Command.add:
						return new AddCommand(_client, GetServiceUri(CalculatorCommand.Command.add));

					case CalculatorCommand.Command.sub:
						return new SubCommand(_client, GetServiceUri(CalculatorCommand.Command.sub));

					case CalculatorCommand.Command.mult:
						return new MultCommand(_client, GetServiceUri(CalculatorCommand.Command.mult));

					case CalculatorCommand.Command.div:
						return new DivCommand(_client, GetServiceUri(CalculatorCommand.Command.div));

					case CalculatorCommand.Command.sqrt:
						return new SqrtCommand(_client, GetServiceUri(CalculatorCommand.Command.sqrt));

					case CalculatorCommand.Command.journal:
						return new JournalCommand(_client, GetServiceUri(CalculatorCommand.Command.journal));

					default:
						break;
				}
			}

			return result;
		}

		private string GetServiceUri(CalculatorCommand.Command command)
		{
			return _configuration.GetValue<string>($"API:URL:{command.ToString().ToUpperInvariant()}");
		}

	}
}
