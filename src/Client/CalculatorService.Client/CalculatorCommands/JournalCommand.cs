using CalculatorService.Model.DTO;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client.CalculatorCommands
{
	internal sealed class JournalCommand : CalculatorCommand
	{
		private const int ARGS_COUNT = 1;

		public JournalCommand(IServiceClient client, string serviceUri)
			: base(client, serviceUri) { }

		protected override async ValueTask<bool> RequestOperation()
		{
			JournalRequest request = new JournalRequest
			{
				Id = _args[0]
			};

			var result = await _client.RequestCalculation(_serviceUri, request, _trackerId);

			if (result.Success && result.Data != null)
			{
				var journalResponse = JsonSerializer.Deserialize<JournalResponse>(result.Data);

				if (journalResponse != null)
				{
					foreach (var operation in journalResponse.Operations)
						_resultBuilder.AppendLine($"Date: {operation?.Date}\tOperation: {operation?.Operation}\t\tCalculation: {operation?.Calculation}");
				}

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
			return true;
		}

		public override string GetCommandHelp()
		{
			StringBuilder help = new StringBuilder();

			help.AppendLine($"{Command.journal} <trackerId>");
			help.AppendLine();
			help.AppendLine("trackerId:\t\tString");
			help.AppendLine();

			return help.ToString();
		}
	}
}
