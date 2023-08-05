using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class AddCommand : CalculatorCommand
	{
		private const int MIN_ARGS = 2;

		public AddCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			AddRequest request = new AddRequest
			{
				Addends = _args.Select(x => ToDouble(x)).ToArray()
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var addResult = JsonSerializer.Deserialize<AddResponse>(result.Data);
				_resultBuilder.AppendLine($"Sum: {addResult?.Sum}");

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
			if (_args.Length < MIN_ARGS)
			{
				_resultBuilder.AppendLine("Insufficient arguments");
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

			help.AppendLine($"{Command.add} [options] <addend-1> <addend-2> [addend-3] ... [addend-N]");
			help.AppendLine();
			help.AppendLine("addend-i:\t\tDecimal number");
			help.AppendLine();
			help.AppendLine(GetOptionsHelp());

			return help.ToString();
		}
	}
}
