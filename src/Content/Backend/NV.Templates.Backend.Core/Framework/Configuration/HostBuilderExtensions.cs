using System;
using System.IO;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

        public static IHostBuilder UseAzureKeyVaultWhenPresent<T>(this IHostBuilder hostBuilder, TimeSpan? reloadInterval = null)
            where T : class
        {
            hostBuilder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var config = configBuilder.Build();
                var keyVaultName = config["KeyVaultName"];

                if (keyVaultName != null)
                {
                    var secretClient = new SecretClient(
                    new Uri($"https://{keyVaultName}.vault.azure.net/"),
                    new DefaultAzureCredential());
                    configBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());

                    configBuilder.AddAzureKeyVault(
                        new SecretClient(
                               new Uri($"https://{keyVaultName}.vault.azure.net/"),
                               new DefaultAzureCredential()),
                        new AzureKeyVaultConfigurationOptions()
                        {
                            ReloadInterval = reloadInterval != null ? reloadInterval : TimeSpan.FromMinutes(5),
                        });
                }
                else if (context.HostingEnvironment.IsDevelopment())
                {
                    configBuilder.AddUserSecrets<T>();
                }
            });
            return hostBuilder;
        }
    }
}
