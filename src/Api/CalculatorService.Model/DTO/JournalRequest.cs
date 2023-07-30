using System.ComponentModel.DataAnnotations;

namespace CalculatorService.Model.DTO
{
	public class JournalRequest
	{
		[Required, MinLength(1)]
		public string Id { get; set; } = string.Empty;
	}
}
