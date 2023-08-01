using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class AddRequest
	{
		[JsonPropertyName("Addends")]
		[Required, MinLength(2)]
		public double[] Addends { get; set; } = new double[0];

		public override string ToString()
		{
			return string.Join(" + ", Addends);
		}
	}
}
