using System.Collections.Generic;
using Newtonsoft.Json;
using NV.Templates.Backend.Core.Framework.Continuation;

namespace NV.Templates.Backend.Web.Framework.Models
{
    /// <summary>
    /// Models that reflects <see cref="IContinuationEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    internal class ContinuationEnumerableModel<T> : IContinuation
    {
        [JsonConstructor]
        public ContinuationEnumerableModel(IEnumerable<T> items, string continuationToken)
        {
            Items = items;
            ContinuationToken = continuationToken;
        }

        public ContinuationEnumerableModel(IContinuationEnumerable<T> continuationEnumerable)
            : this(continuationEnumerable, continuationEnumerable.ContinuationToken)
        {
        }

        public IEnumerable<T> Items { get; }

        public string ContinuationToken { get; }
    }
}
