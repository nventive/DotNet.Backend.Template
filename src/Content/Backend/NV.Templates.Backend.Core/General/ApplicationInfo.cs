using System.Reflection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IApplicationInfo"/> implementation.
    /// </summary>
    internal class ApplicationInfo : IApplicationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> class using current
        /// <see cref="AssemblyInformationalVersionAttribute"/> and the <paramref name="hostingEnvironment"/>.
        /// </summary>
        public ApplicationInfo(IHostingEnvironment hostingEnvironment)
            : this(
                  hostingEnvironment?.ApplicationName ?? typeof(ApplicationInfo).Assembly.GetName().Name,
                  typeof(ApplicationInfo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                  hostingEnvironment?.EnvironmentName ?? EnvironmentName.Production)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> class.
        /// </summary>
        /// <param name="name">Application name.</param>
        /// <param name="version">Application version.</param>
        /// <param name="environment">Application environment.</param>
        [JsonConstructor]
        public ApplicationInfo(string name, string version, string environment)
        {
            Name = name;
            Version = version;
            Environment = environment;
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
