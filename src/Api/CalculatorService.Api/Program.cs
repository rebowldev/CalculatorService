using CalculatorService.Api.Filters;
using CalculatorService.Api.Middlewares;
using CalculatorService.Application;
using CalculatorService.Interfaces.Application;
using CalculatorService.Interfaces.Infrastructure;
using CalculatorService.Model.DTO;
using CalculatorService.Tracker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICalculator, Calculator>();
builder.Services.AddSingleton<ITrackerService<OperationInfo>, InMemoryTrackerService<OperationInfo>>();

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => config.OperationFilter<JournalTrackingFilter>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Uses Journal Middleware to track operations when tracking ID is give within request headers
app.UseJournalMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
