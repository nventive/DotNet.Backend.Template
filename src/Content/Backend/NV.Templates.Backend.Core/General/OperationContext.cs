using System;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using NV.Templates.Backend.Core.Framework.DependencyInjection;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IOperationContext"/> implementation.
    /// </summary>
    [RegisterService(ServiceLifetime.Scoped)]
    internal class OperationContext : IOperationContext
    {
        /// <inheritdoc/>
        public string Id { get; set; } = $"{Guid.NewGuid()}";

        /// <inheritdoc/>
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public IPrincipal? User { get; set; } = Framework.Auth.SystemPrincipal.Instance;

        /// <inheritdoc />
        public override string ToString() => $"{nameof(OperationContext)}: {Id} {Timestamp} {User?.Identity?.Name}";
    }
}
