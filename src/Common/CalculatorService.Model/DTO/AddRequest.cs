using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CalculatorService.Model.DTO
{
	public class AddRequest
	{
		[XmlIgnore]
		[JsonPropertyName("Addends")]
		[Required, MinLength(2)]
		public double[] Addends { get; set; } = new double[0];

		[JsonIgnore]
		[XmlElement("Addends")]
		public string AddendsArray
		{
			get
			{
				return string.Join(" ", Addends);
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Addends = value
						.Split(" ", StringSplitOptions.RemoveEmptyEntries)
						.Select(x => double.Parse(x)).ToArray();
				}
			}
		}

		public override string ToString()
		{
			return string.Join(" + ", Addends);
		}
	}
}
