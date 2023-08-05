using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class SqrtCommand : CalculatorCommand
	{
		private const int ARGS_COUNT = 1;

		public SqrtCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			SqrtRequest request = new SqrtRequest
			{
				Number = ToDouble(_args[0])
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var sqrtResponse = JsonSerializer.Deserialize<SqrtResponse>(result.Data);
				_resultBuilder.AppendLine($"Square: {sqrtResponse?.Square}");

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
			if (_args.Length == 0)
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
				_resultBuilder.AppendLine("Invalid type: Argument must be an integer or decimal number");
				return false;
			}

			return true;
		}

		public override string GetCommandHelp()
		{
			StringBuilder help = new StringBuilder();

			help.AppendLine($"{Command.sqrt} [options] <number>");
			help.AppendLine();
			help.AppendLine("number:\t\tDecimal number");
			help.AppendLine();
			help.AppendLine(GetOptionsHelp());

			return help.ToString();
		}
	}
}
