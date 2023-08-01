using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class AddResponse
	{
		[JsonPropertyName("Sum")]
		public double Sum { get; set; }

		public override string ToString()
		{
			return Sum.ToString();
		}
	}
}
