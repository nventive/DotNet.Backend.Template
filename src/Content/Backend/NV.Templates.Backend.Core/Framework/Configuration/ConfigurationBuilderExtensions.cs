using System;
using System.IO;
using System.Reflection;
using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// <see cref="IConfigurationBuilder"/> extension methods.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the root LocalSettings.Development.json file as a configuration source.
        /// </summary>
        public static IConfigurationBuilder AddLocalSettings(
            this IConfigurationBuilder config,
            string rootProjectPath)
        {
            config.AddJsonFile(
                Path.Combine(rootProjectPath, "LocalSettings.Development.json"),
                optional: true,
                reloadOnChange: true);

            return config;
        }

        /// <summary>
        /// Adds KeyVault as a source of configuration when a configuration key named "KeyVault" is present.
        /// The "KeyVault" configuration key must contain the KeyVault URL.
        /// The connection is made using Azure System Managed Identities.
        /// See https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-3.0#use-managed-identities-for-azure-resources
        /// for more information.
        /// </summary>
        public static IConfigurationBuilder AddAzureKeyVaultWhenPresent(this IConfigurationBuilder config, HostBuilderContext context)
        {
            var builtConfig = config.Build();
            var keyVault = builtConfig["KeyVault"];

            if (!string.IsNullOrWhiteSpace(keyVault))
            {
                var tokenCredential = builtConfig.GetTokenCredential();

                var secretClient = new SecretClient(new Uri(keyVault), tokenCredential);

                config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            }
            else if (context.HostingEnvironment.IsDevelopment())
            {
                config.AddUserSecrets(Assembly.GetExecutingAssembly());
            }

            return config;
        }

        /// <summary>
        /// Get Token credential with Managed Identity
        ///  https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview.
        /// </summary>
        /// <returns>Azure Credential. (Azure = SystemManager identity , VS = signed in account ) ... </returns>
        public static TokenCredential GetTokenCredential(this IConfiguration builtConfig)
        {
            // return new ManagedIdentityCredential(clientId); // Explicit Credential for User Assigned Mananged Identity
            return new DefaultAzureCredential();
        }
    }
}
