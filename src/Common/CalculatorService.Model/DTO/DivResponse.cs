using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class DivResponse
	{
		[JsonPropertyName("Quotient")]
		public int Quotient { get; set; }

		[JsonPropertyName("Remainder")]
		public int Remainder { get; set; }

		public override string ToString()
		{
			return $"{Quotient} ({Remainder})";
		}
	}
}
