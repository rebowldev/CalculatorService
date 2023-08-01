using CalculatorService.Server.Middlewares;

namespace CalculatorService.Server.Extensions
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
