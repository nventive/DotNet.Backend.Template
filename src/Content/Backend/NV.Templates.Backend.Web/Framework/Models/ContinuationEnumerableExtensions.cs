using System;
using System.Linq;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.Web.Framework.Models
{
    /// <summary>
    /// <see cref="IContinuationEnumerable{T}"/> extension methods.
    /// </summary>
    internal static class ContinuationEnumerableExtensions
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
