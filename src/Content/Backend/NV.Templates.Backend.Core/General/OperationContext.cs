using System;
using NV.Templates.Backend.Core.Framework;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// <see cref="IOperationContext"/> implementation.
    /// </summary>
    internal class OperationContext : IOperationContext
    {
        /// <inheritdoc />
        public string OperationId { get; set; } = IdGenerator.Generate();

        /// <inheritdoc />
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public override string ToString() => $"{nameof(OperationContext)}: {OperationId} {Timestamp}";
    }
}
