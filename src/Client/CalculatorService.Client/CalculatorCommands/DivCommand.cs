using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class DivCommand : CalculatorCommand
	{
		private const int ARGS_COUNT = 2;

		public DivCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			DivRequest request = new DivRequest
			{
				Dividend = int.Parse(_args[0]),
				Divisor = int.Parse(_args[1])
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var divResponse = JsonSerializer.Deserialize<DivResponse>(result.Data);
				_resultBuilder.AppendLine($"Quotient: {divResponse?.Quotient}, Remainder: {divResponse?.Remainder}");

				return true;
			}
			else
			{
				_resultBuilder.AppendLine(result.Message);
				return false;
			}
		}

		protected override bool ValidateArgumentsCount()
		{
			if (_args.Length < ARGS_COUNT)
			{
				_resultBuilder.AppendLine("Insufficient arguments");
				return false;
			}

			if (_args.Length > ARGS_COUNT)
			{
				_resultBuilder.AppendLine("Too many arguments");
				return false;
			}

			return true;
		}

		protected override bool ValidateArgumentsType()
		{
			if (_args.Any(arg => !long.TryParse(arg, out var number)))
			{
				_resultBuilder.AppendLine("Invalid type: Arguments must be integers");
				return false;
			}

			return true;
		}

		public override string GetCommandHelp()
		{
			StringBuilder help = new StringBuilder();

			help.AppendLine($"{Command.div} [options] <dividend> <divisor>");
			help.AppendLine();
			help.AppendLine("dividend:\t\tInteger number");
			help.AppendLine("divisor:\t\tInteger number");
			help.AppendLine();
			help.AppendLine(GetOptionsHelp());

			return help.ToString();
		}
	}
}
