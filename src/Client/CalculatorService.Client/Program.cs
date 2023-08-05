using CalculatorService.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appSettings.json", false)
		.Build();

var serviceProvider = new ServiceCollection()
	.AddSingleton(configuration)
	.AddHttpClient()
	.AddSingleton<IConsoleWrapper, ConsoleWrapper>()
	.AddSingleton<IServiceClient, ServiceClient>()
	.AddSingleton<CommandLineManager>()
	.BuildServiceProvider();

var commandLineManager = serviceProvider.GetRequiredService<CommandLineManager>();
commandLineManager.Run();

