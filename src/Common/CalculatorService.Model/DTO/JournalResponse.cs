using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class JournalResponse
	{ 
		public JournalResponse()
		{
			Operations = new OperationInfo[0];
		}

		public JournalResponse(OperationInfo[] operations)
		{
			Operations = operations ?? new OperationInfo[0];
		}

		[JsonPropertyName("Operations")]
		public OperationInfo[] Operations { get; set; }
	}

	public class OperationInfo
	{
		[JsonPropertyName("Operation")]
		public string Operation { get; set; } = string.Empty;

		[JsonPropertyName("Calculation")]
		public string Calculation { get; set; } = string.Empty;

		[JsonPropertyName("Date")]
		public DateTime Date { get; set; }
	}
}
