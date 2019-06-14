using System;
using System.Linq;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.RestApi.Framework.Models
{
    /// <summary>
    /// <see cref="IContinuationEnumerable{T}"/> extension methods.
    /// </summary>
    public static class ContinuationEnumerableExtensions
    {
        public static ContinuationEnumerableModel<TSource> ToModel<TSource>(this IContinuationEnumerable<TSource> source)
        {
            return new ContinuationEnumerableModel<TSource>(source);
        }

        public static ContinuationEnumerableModel<TProjected> ToModel<TSource, TProjected>(this IContinuationEnumerable<TSource> source, Func<TSource, TProjected> select)
        {
            return new ContinuationEnumerableModel<TProjected>(source.Select(select), source.ContinuationToken);
        }
    }
}
