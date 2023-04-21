using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IApplicationInfo"/> implementation.
    /// </summary>
    [RegisterService(ServiceLifetime.Singleton)]
    internal class ApplicationInfo : IApplicationInfo
    {
        public ApplicationInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> class using current
        /// <see cref="AssemblyInformationalVersionAttribute"/> and the <paramref name="hostEnvironment"/>.
        /// </summary>
        public ApplicationInfo(IHostEnvironment hostEnvironment)
        {
            Name = hostEnvironment?.ApplicationName ?? (typeof(ApplicationInfo).Assembly.GetName().Name ?? string.Empty);
            Version = typeof(ApplicationInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty;
            Environment = hostEnvironment?.EnvironmentName ?? Environments.Production;
        }

        /// <inheritdoc />
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Version { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Environment { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() => $"{nameof(ApplicationInfo)}: {Name} ({Version}) / {Environment}";
    }
}
