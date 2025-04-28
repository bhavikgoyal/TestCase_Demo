using ExternalUserIntegration.Clients;
using ExternalUserIntegration.Configuration;
using ExternalUserIntegration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

// Setup Configuration
var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json", optional: true)
	.Build();

// Bind configuration to ReqresApiOptions
services.Configure<ReqresApiOptions>(options =>
{
	// Ensure "ReqresApiOptions" section exists in appsettings.json
	configuration.GetSection(nameof(ReqresApiOptions)).Bind(options);
});

// Setup HttpClient
services.AddHttpClient<IReqresApiClient, ReqresApiClient>()
	.ConfigureHttpClient(client =>
	{
		client.BaseAddress = new Uri("https://reqres.in"); // Set base URL here
	});

// Setup Services
services.AddScoped<IExternalUserService, ExternalUserService>();
services.AddScoped<IReqresApiClient, ReqresApiClient>();


var provider = services.BuildServiceProvider();

var userService = provider.GetRequiredService<IExternalUserService>();

// Fetch all users
var users = await userService.GetAllUsersAsync();

foreach (var user in users)
{
	Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName} ({user.Email})");
}
