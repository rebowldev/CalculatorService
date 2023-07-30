using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class AddResponse
	{
		[JsonPropertyName("sum")]
		public double Sum { get; set; }

		public override string ToString()
		{
			return Sum.ToString();
		}
	}
}
