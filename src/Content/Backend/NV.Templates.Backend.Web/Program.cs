using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NV.Templates.Backend.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLocalSettingsInDevelopment()
                .UseAzureAppConfigurationWhenPresent()
                .UseAzureKeyVaultWhenPresent<Startup>()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options => options.AddServerHeader = false)
                        .UseStartup<Startup>();
                });
    }
}
