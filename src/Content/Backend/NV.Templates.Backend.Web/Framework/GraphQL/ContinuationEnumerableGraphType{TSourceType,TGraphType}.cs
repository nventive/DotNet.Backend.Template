using System;
using System.Linq;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.Web.Framework.GraphQL
{
    /// <summary>
    /// Maps <see cref="IContinuationEnumerable{T}"/> to an <see cref="ObjectGraphType"/>.
    /// </summary>
    /// <typeparam name="TSourceType">The Type parameter of the enumeration.</typeparam>
    /// <typeparam name="TGraphType">The target <see cref="ObjectGraphType"/>.</typeparam>
    internal abstract class ContinuationEnumerableGraphType<TSourceType, TGraphType> : ObjectGraphType<IContinuationEnumerable<TSourceType>>
        where TGraphType : GraphType
    {
        public ContinuationEnumerableGraphType(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<TGraphType>>>>()
                .Name(ContinuationEnumerableGraphType.NodesFieldName)
                .Description("The retrieved nodes for the current continuation token.")
                .Resolve(context => context.Source.AsEnumerable());
            Field<StringGraphType>()
                .Name(ContinuationEnumerableGraphType.ContinuationTokenFieldName)
                .Description("The next continuation token to pass back to get the rest.")
                .Resolve(context => context.Source.ContinuationToken);
        }
    }
}
