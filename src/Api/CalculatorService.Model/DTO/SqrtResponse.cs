using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class SqrtResponse
	{
		[JsonPropertyName("Square")]
		public double Square { get; set; }

		public override string ToString()
		{
			return Square.ToString();
		}
	}
}
