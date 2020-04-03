using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NV.Templates.Backend.Core.Tests.Framework.Continuation;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Helps with loading IOptions.
    /// </summary>
    public static class OptionsHelper
    {
        /// <summary>
        /// Loads <typeparamref name="T"/> as options using the appsettings.json, Environment variables and User secrets.
        /// </summary>
        /// <typeparam name="T">The options type.</typeparam>
        /// <param name="key">The section name. Defaults to typeof(T).Name (minus the -Options suffix).</param>
        public static IOptions<T> GetOptionsFromConfig<T>(string? key = null)
            where T : class, new()
        {
            var configuration = GetConfiguration();
            key ??= RegistrationServiceCollectionExtensions.DefaultOptionsName<T>();
            return Options.Create(configuration.GetSection(key).Get<T>() ?? new T());
        }

        /// <summary>
        /// Creates a <see cref="IConfigurationRoot"/> using the appsettings.json and Environment variables.
        /// </summary>
        public static IConfigurationRoot GetConfiguration()
        {
            var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
            return new ConfigurationBuilder()
                .AddLocalSettings(rootProjectPath)
                .AddEnvironmentVariables()
                .AddUserSecrets<ContinuationTokenTests>()
                .Build();
        }
    }
}
