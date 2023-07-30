namespace CalculatorService.Model.DTO
{
	public class JournalResponse
	{
		public JournalResponse(List<OperationInfo> operations)
		{
			Operations = operations ?? new List<OperationInfo>();
		}

		public List<OperationInfo> Operations { get; set; } = new List<OperationInfo>();
	}

	public class OperationInfo
	{
		public string Operation { get; set; } = string.Empty;
		public string Calculation { get; set; } = string.Empty;
		public DateTime Date { get; set; }
	}
}
