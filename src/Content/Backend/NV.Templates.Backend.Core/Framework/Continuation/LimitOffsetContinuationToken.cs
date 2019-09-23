namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Continuation token support adapted for offset/limit paginations systems.
    /// </summary>
    internal class LimitOffsetContinuationToken
    {
        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        public int Limit { get; set; } = ContinuationQuery.DefaultLimit;

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Convert a base64 string to a continuation token.
        /// </summary>
        /// <param name="query">The <see cref="IContinuationQuery"/> to use.</param>
        /// <returns>A <see cref="LimitOffsetContinuationToken"/>.</returns>
        public static LimitOffsetContinuationToken FromContinuationQuery(IContinuationQuery query)
        {
            return ContinuationToken.FromContinuationToken<LimitOffsetContinuationToken>(query.ContinuationToken)
                ?? new LimitOffsetContinuationToken { Limit = query.Limit };
        }

        /// <summary>
        /// Get the next page token.
        /// </summary>
        /// <returns>A bse64 encoded next page token.</returns>
        public string GetNextPageContinuationToken()
        {
            return ContinuationToken.ToContinuationToken(
                new LimitOffsetContinuationToken { Offset = Offset + Limit, Limit = Limit });
        }
    }
}
