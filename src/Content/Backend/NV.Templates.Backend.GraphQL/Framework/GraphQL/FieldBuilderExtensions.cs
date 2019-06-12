using GraphQL.Builders;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// Extension methods for <see cref="FieldBuilder"/>
    /// </summary>
    internal static class FieldBuilderExtensions
    {
        public const string ContinuationTokenArgumentName = "continuationToken";
        public const string LimitArgumentName = "limit";

        /// <summary>
        /// Adds <see cref="ContinuationTokenArgumentName"/> and <see cref="LimitArgumentName"/>
        /// as arguments.
        /// </summary>
        /// <typeparam name="TSourceType">The graph source type</typeparam>
        /// <typeparam name="TReturnType">The actual returned type</typeparam>
        public static FieldBuilder<TSourceType, TReturnType> ArgumentContinuationQuery<TSourceType, TReturnType>(this FieldBuilder<TSourceType, TReturnType> fieldBuilder)
        {
            return fieldBuilder
                .Argument<IntGraphType, int>(LimitArgumentName, "Number of nodes to return", ContinuationQuery.DefaultLimit)
                .Argument<StringGraphType>(ContinuationTokenArgumentName, "The previous continuation token");
        }
    }
}
