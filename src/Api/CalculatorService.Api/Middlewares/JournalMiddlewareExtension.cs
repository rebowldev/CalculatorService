namespace CalculatorService.Api.Middlewares
{
	public static class JournalMiddlewareExtension
	{
		public static IApplicationBuilder UseJournalMiddleware(
		this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<JournalMiddleware>();
		}
	}
}
