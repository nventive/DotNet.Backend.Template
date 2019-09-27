using System;
#if Auth
using System.Security.Principal;
#endif
using NV.Templates.Backend.Core.Framework;
using NV.Templates.Backend.Core.Framework.Services;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IOperationContext"/> implementation.
    /// </summary>
    [RegisterScopedService]
    internal class OperationContext : IOperationContext
    {
        /// <inheritdoc />
        public string OperationId { get; set; } = IdGenerator.Generate();

        /// <inheritdoc />
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

#if Auth
        /// <inheritdoc />
        public IIdentity? UserIdentity { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{nameof(OperationContext)}: {OperationId} {Timestamp} {UserIdentity.GetUserName()}";
#else
        /// <inheritdoc />
        public override string ToString() => $"{nameof(OperationContext)}: {OperationId} {Timestamp}";
#endif
    }
}
