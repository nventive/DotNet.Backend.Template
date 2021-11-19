using System.IO;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// <see cref="IHostBuilder"/> extension methods.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds the root LocalSettings.Development.json file as a configuration source.
        /// Used only when the environment is "Development".
        /// </summary>
        public static IHostBuilder UseLocalSettingsInDevelopment(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.AddLocalSettings(
                        Path.Combine(context.HostingEnvironment.ContentRootPath, ".."));
                }
            });

            return hostBuilder;
        }

        /// <summary>
        /// Adds Azure App Configuration as a source of configuration when a connection string named "AppConfig" is present.
        /// </summary>
        public static IHostBuilder UseAzureAppConfigurationWhenPresent(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                var buildConfig = config.Build();
                var appConfigConnectionString = buildConfig.GetConnectionString("AppConfig");
                if (!string.IsNullOrWhiteSpace(appConfigConnectionString))
                {
                    config.AddAzureAppConfiguration(appConfigConnectionString);
                }
            });

            return hostBuilder;
        }

        /// <summary>
        /// Adds KeyVault as a source of configuration when a configuration key named "KeyVault" is present.
        /// The "KeyVault" configuration key must contain the KeyVault URL.
        /// The connection is made using Azure System Managed Identities.
        /// See https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-3.0#use-managed-identities-for-azure-resources
        /// for more information.
        /// </summary>
        public static IHostBuilder UseKeyVaultWithManagedIdentityWhenPresent(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddAzureKeyVaultWhenPresent(context);
            });
            return hostBuilder;
        }
    }
}
