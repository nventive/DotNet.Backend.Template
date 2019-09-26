namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Represents standard parameters for continuation token support in queries.
    /// </summary>
    public interface IContinuationQuery
    {
        /// <summary>
        /// Gets or sets the continuation token.
        /// </summary>
        string ContinuationToken { get; set; }

        /// <summary>
        /// Gets or sets the number of items to fetch.
        /// </summary>
        int Limit { get; set; }
    }
}
