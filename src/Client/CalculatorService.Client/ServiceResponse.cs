namespace CalculatorService.Client
{
	public class ServiceResponse<T> where T : class
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public T? Data { get; set; }

		public static ServiceResponse<T> Sucess(T data)
		{
			return new ServiceResponse<T>
			{
				Success = true,
				Data = data
			};
		}

		public static ServiceResponse<T> Error(string message)
		{
			return new ServiceResponse<T>
			{
				Success = false,
				Message = message
			};
		}
	}
}
