using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class MultCommand : CalculatorCommand
	{
		private const int MIN_ARGS = 2;

		public MultCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			MultRequest request = new MultRequest
			{
				Factors = _args.Select(x => ToDouble(x)).ToArray()
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var multResponse = JsonSerializer.Deserialize<MultResponse>(result.Data);
				_resultBuilder.AppendLine($"Product: {multResponse?.Product}");

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

			help.AppendLine($"{Command.mult} [options] <factor-1> <factor-2> [factor-3] ... [factor-N]");
			help.AppendLine();
			help.AppendLine("factor-i:\t\tDecimal number");
			help.AppendLine();
			help.AppendLine(GetOptionsHelp());

			return help.ToString();
		}
	}
}
