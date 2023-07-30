using CalculatorService.Api.Middlewares;

namespace CalculatorService.Api.Extensions
{
	public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestHandlerMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestHandlerMiddleware>();
        }
    }
}
