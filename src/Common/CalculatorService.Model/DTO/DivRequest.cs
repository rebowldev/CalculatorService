using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class DivRequest
	{
		[JsonPropertyName("Dividend")]
		[Required]
		public int Dividend { get; set; }

		[JsonPropertyName("Divisor")]
		[Required]
		public int Divisor { get; set; }

		public override string ToString()
		{
			return $"{Dividend} / {Divisor}";
		}
	}
}
