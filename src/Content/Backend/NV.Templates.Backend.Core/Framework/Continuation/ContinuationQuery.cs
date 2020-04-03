namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Base implementation for <see cref="IContinuationQuery"/>.
    /// </summary>
    public abstract class ContinuationQuery : IContinuationQuery
    {
        /// <summary>
        /// Gets the default limit to apply when none provided.
        /// </summary>
        public static readonly int DefaultLimit = 50;

        /// <inheritdoc />
        public int Limit { get; set; } = DefaultLimit;

        /// <inheritdoc />
        public string? ContinuationToken { get; set; }
    }
}
