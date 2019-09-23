using System;

namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// The context for the current operation.
    /// </summary>
    public interface IOperationContext
    {
        /// <summary>
        /// Gets or sets the current operation id.
        /// </summary>
        string OperationId { get; set; }

        /// <summary>
        /// Gets or sets the operation timestamp in UTC.
        /// </summary>
        DateTimeOffset Timestamp { get; set; }
    }
}
