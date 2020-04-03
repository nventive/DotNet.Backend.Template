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
    }
}
