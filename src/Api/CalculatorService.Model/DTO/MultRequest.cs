using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class MultRequest
	{
		[JsonPropertyName("Factors")]
		[Required, MinLength(2)]
		public double[] Factors { get; set; } = new double[0];

		public override string ToString()
		{
			return string.Join(" * ", Factors);
		}
	}
}
