using System;
using System.Linq;
using GraphQL.Language.AST;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.Web.Framework.GraphQL
{
    /// <summary>
    /// Extension methods for <see cref="ResolveFieldContext"/>.
    /// </summary>
    internal static class ResolveFieldContextExtensions
    {
        /// <summary>
        /// Fills <paramref name="query"/> with GraphQL arguments previously setup with
        /// <see cref="FieldBuilderExtensions.ArgumentContinuationQuery{TSourceType, TReturnType}(GraphQL.Builders.FieldBuilder{TSourceType, TReturnType})"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        public static ResolveFieldContext<TSource> FillContinuationQuery<TSource>(this ResolveFieldContext<TSource> context, IContinuationQuery query)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));
            query.Limit = context.GetArgument<int>(FieldBuilderExtensions.LimitArgumentName);
            query.ContinuationToken = context.GetArgument<string>(FieldBuilderExtensions.ContinuationTokenArgumentName);

            return context;
        }

        /// <summary>
        /// Returns true if the context includes a sub-node of <paramref name="nodeName"/>
        /// in its "nodes" (<see cref="ContinuationEnumerableGraphType.NodesFieldName"/>) sub-node.
        /// It allows the detection of sub-node in a query to avoid SELECT N+1 in some scenarios.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        public static bool HasSubNode<TSource>(this ResolveFieldContext<TSource> context, string nodeName)
        {
            if (context.SubFields.ContainsKey(ContinuationEnumerableGraphType.NodesFieldName))
            {
                var nodesField = context.SubFields[ContinuationEnumerableGraphType.NodesFieldName];
                var selectionSet = nodesField.Children.FirstOrDefault(x => x is SelectionSet);

                if (selectionSet != null)
                {
                    return selectionSet.Children
                        .OfType<Field>()
                        .Any(field => string.Equals(field.Name, nodeName, StringComparison.OrdinalIgnoreCase));
                }
            }

            return false;
        }
    }
}
