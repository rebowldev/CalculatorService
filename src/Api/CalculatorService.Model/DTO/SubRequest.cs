using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class SubRequest
	{
		[JsonPropertyName("Minuend")]
		[Required]
		public double Minuend { get; set;}

		[JsonPropertyName("Subtrahend")]
		[Required]
		public double Subtrahend { get; set; }

		public override string ToString()
		{
			return $"{Minuend} - {Subtrahend}";
		}
	}
}
