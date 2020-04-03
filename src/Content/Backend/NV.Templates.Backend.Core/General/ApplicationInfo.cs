using System.Reflection;
using Microsoft.Extensions.Hosting;
using NV.Templates.Backend.Core.Framework.DependencyInjection;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IApplicationInfo"/> implementation.
    /// </summary>
    [RegisterSingletonService]
    internal class ApplicationInfo : IApplicationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> class using current
        /// <see cref="AssemblyInformationalVersionAttribute"/> and the <paramref name="hostEnvironment"/>.
        /// </summary>
        public ApplicationInfo(IHostEnvironment hostEnvironment)
        {
            Name = hostEnvironment?.ApplicationName ?? typeof(ApplicationInfo).Assembly.GetName().Name;
            Version = typeof(ApplicationInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            Environment = hostEnvironment?.EnvironmentName ?? Environments.Production;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Version { get; }

        /// <inheritdoc />
        public string Environment { get; }

        /// <inheritdoc />
        public override string ToString() => $"{nameof(ApplicationInfo)}: {Name} ({Version}) / {Environment}";
    }
}
