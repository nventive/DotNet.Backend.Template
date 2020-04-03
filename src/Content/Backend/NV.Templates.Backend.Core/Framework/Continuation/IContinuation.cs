namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Indicates that it contains a continuation token.
    /// </summary>
    public interface IContinuation
    {
        /// <summary>
        /// Gets the continuation token.
        /// </summary>
        string? ContinuationToken { get; }
    }
}
