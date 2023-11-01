using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
        var hostingEnvironment = services.BuildServiceProvider().GetService<IHostEnvironment>();
        var configuration = new ConfigurationBuilder()
            .AddLocalSettings(rootProjectPath)
            .AddAzureKeyVaultWhenPresent(hostingEnvironment!)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>(true)
            .Build();
        services.AddSingleton(configuration);
        services.AddCore(configuration);
    })
    .Build();

await host.RunAsync();
