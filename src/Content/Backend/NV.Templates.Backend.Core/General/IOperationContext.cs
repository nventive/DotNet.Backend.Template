using System;
using System.Security.Principal;

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
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the operation timestamp in UTC.
        /// </summary>
        DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IPrincipal"/>, if any.
        /// </summary>
        IPrincipal? User { get; set; }
    }
}
