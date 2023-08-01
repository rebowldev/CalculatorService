using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CalculatorService.Server.Filters
{
	public class JournalTrackingFilter : IOperationFilter
	{
		private readonly string _headerKey;

		public JournalTrackingFilter(ITrackerService<OperationInfo> trackerService)
		{
			_headerKey = trackerService.HeaderKey;
		}

		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context.ApiDescription.RelativePath.StartsWith("calculator"))
			{
				if (operation.Parameters == null)
					operation.Parameters = new List<OpenApiParameter>();

				operation.Parameters.Add(new OpenApiParameter()
				{
					Name = _headerKey,
					In = ParameterLocation.Header,
					Required = false,
					Schema = new OpenApiSchema() { Type = "string" }
				});
			}
		}
	}
}
