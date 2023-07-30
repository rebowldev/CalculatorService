using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class SubResponse
	{
		[JsonPropertyName("Difference")]
		public double Difference { get; set; }

		public override string ToString()
		{
			return Difference.ToString();
		}
	}
}
