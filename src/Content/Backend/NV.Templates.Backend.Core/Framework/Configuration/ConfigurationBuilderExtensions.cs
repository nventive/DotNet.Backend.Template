using System.IO;

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
    }
}
