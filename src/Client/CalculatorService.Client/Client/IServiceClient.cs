namespace CalculatorService.Client
{
	public interface IServiceClient
	{
		Task<ServiceResponse<string>> RequestCalculation<T>(string requestUri, T request, string trackerId) where T : class;
	}
}
