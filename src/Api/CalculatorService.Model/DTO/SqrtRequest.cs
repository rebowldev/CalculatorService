using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class SqrtRequest
	{
		[JsonPropertyName("Number")]
		[Required]
		public double Number { get; set; }

		public override string ToString()
		{
			return $"SQRT({Number})";
		}
	}
}
