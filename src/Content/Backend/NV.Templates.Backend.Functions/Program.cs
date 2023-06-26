using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
        var hostingEnvironment = builder.Services.BuildServiceProvider().GetService<IHostEnvironment>();
        var configuration = new ConfigurationBuilder()
            .AddLocalSettings(rootProjectPath)
            .AddAzureKeyVaultWhenPresent(hostingEnvironment!)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>(true)
            .Build();

        builder.Services.AddSingleton(configuration);
        builder.Services.AddCore(configuration);
    })
    .Build();

await host.RunAsync();
