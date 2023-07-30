using System.Text.Json.Serialization;

namespace CalculatorService.Model.DTO
{
	public class MultResponse
	{
		[JsonPropertyName("Product")]
		public double Product { get; set; }

		public override string ToString()
		{
			return Product.ToString();
		}
	}
}
