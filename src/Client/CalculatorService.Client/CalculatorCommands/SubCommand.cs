using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class SubCommand : CalculatorCommand
	{
		private const int ARGS_COUNT = 2;

		public SubCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			SubRequest request = new SubRequest
			{
				Minuend = ToDouble(_args[0]),
				Subtrahend = ToDouble(_args[1])
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var subResponse = JsonSerializer.Deserialize<SubResponse>(result.Data);
				_resultBuilder.AppendLine($"Difference: {subResponse?.Difference}");

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
			if (_args.Any(arg => !double.TryParse(arg, out var number)))
			{
				_resultBuilder.AppendLine("Invalid type: Arguments must be integers or decimal numbers");
				return false;
			}

			return true;
		}

		public override string GetCommandHelp()
		{
			StringBuilder help = new StringBuilder();

			help.AppendLine($"{Command.sub} [options] <minuend> <subtrahend>");
			help.AppendLine();
			help.AppendLine("minuend:\t\tDecimal number");
			help.AppendLine("subtrahend:\t\tDecimal number");
			help.AppendLine();
			help.AppendLine(GetOptionsHelp());

			return help.ToString();
		}
	}
}
