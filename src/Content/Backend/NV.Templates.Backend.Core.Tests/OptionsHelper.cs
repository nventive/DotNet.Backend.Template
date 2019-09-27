using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core.Tests.Framework.Continuation;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Helps with loading IOptions from appsettings.json.
    /// </summary>
    public static class OptionsHelper
    {
        /// <summary>
        /// Loads <typeparamref name="T"/> as options using the appsettings.json and Environment variables.
        /// </summary>
        /// <typeparam name="T">The options type.</typeparam>
        /// <param name="key">The section name. Defaults to typeof(T).Name.</param>
        public static T GetOptionsFromConfig<T>(string? key = null)
            where T : class, new()
        {
            var configuration = GetConfiguration();
            var options = new T();
            configuration.GetSection(key ?? typeof(T).Name).Bind(options);
            return options;
        }

        /// <summary>
        /// Creates a <see cref="IConfigurationRoot"/> using the appsettings.json and Environment variables.
        /// </summary>
        public static IConfigurationRoot GetConfiguration()
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<ContinuationTokenTests>()
                .Build();
    }
}
