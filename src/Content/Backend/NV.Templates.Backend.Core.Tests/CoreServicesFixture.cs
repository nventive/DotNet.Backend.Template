using System;
using System.IO;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Supports creation and retrieval of services and configuration under tests.
    /// </summary>
    public class CoreServicesFixture : ITestOutputHelperAccessor
    {
        private readonly Lazy<IHost> _host;

        public CoreServicesFixture()
        {
            _host = new Lazy<IHost>(GetTestHost);
        }

        /// <summary>
        /// Gets or sets XUnit's <see cref="ITestOutputHelper"/> for logging.
        /// </summary>
        public ITestOutputHelper? OutputHelper { get; set; }

        /// <summary>
        /// Configure the Core Services and allows instantiation of services in the context of testing.
        /// </summary>
        /// <returns>The <see cref="IServiceScope"/>.</returns>
        public IServiceScope GetServiceScope() => _host.Value.Services.CreateScope();

        /// <summary>
        /// Create a new test <see cref="IHost"/>
        /// with Core services registered and test configuration loaded.
        /// </summary>
        private IHost GetTestHost()
        {
            return Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
                    config
                        .AddLocalSettings(rootProjectPath)
                        .AddJsonFile(
                            Path.Combine(rootProjectPath, "LocalSettings.Test.json"),
                            optional: true,
                            reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<CoreServicesFixture>();
                })
                .ConfigureLogging(logging =>
                {
                    if (OutputHelper != null)
                    {
                        logging.AddXUnit(OutputHelper);
                    }

                    logging.AddFilter((category, level) => category!.EndsWith("TraceHandler", StringComparison.OrdinalIgnoreCase));
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddCore(context.Configuration)
                        .AddHttpRecorderContextSupport();
                })
                .Build();
        }
    }
}
