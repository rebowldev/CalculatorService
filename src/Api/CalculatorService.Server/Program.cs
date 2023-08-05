using CalculatorService.Application;
using CalculatorService.Interfaces.Application;
using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using CalculatorService.Server.Extensions;
using CalculatorService.Server.Filters;
using CalculatorService.Tracker;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICalculator, Calculator>();
builder.Services.AddSingleton<ITrackerService<OperationInfo>, InMemoryTrackerService<OperationInfo>>();

builder.Services.AddControllers()
	.AddXmlSerializerFormatters()
	.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => config.OperationFilter<JournalTrackingFilter>());

builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Use middleware that handles loggin and operations tracking
app.UseRequestHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
