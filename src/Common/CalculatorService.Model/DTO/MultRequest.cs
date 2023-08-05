using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CalculatorService.Model.DTO
{
	public class MultRequest
	{
		[XmlIgnore]
		[JsonPropertyName("Factors")]
		[Required, MinLength(2)]
		public double[] Factors { get; set; } = new double[0];

		[JsonIgnore]
		[XmlElement("Addends")]
		public string FactorsArray
		{
			get
			{
				return string.Join(" ", Factors);
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Factors = value
						.Split(" ", StringSplitOptions.RemoveEmptyEntries)
						.Select(x => double.Parse(x)).ToArray();
				}
			}
		}

		public override string ToString()
		{
			return string.Join(" * ", Factors);
		}
	}
}
