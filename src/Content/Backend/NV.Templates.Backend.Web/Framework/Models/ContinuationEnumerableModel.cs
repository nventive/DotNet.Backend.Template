using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Pantry.Continuation;

namespace NV.Templates.Backend.Web.Framework.Models
{
    /// <summary>
    /// Models that reflects <see cref="IContinuationEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public class ContinuationEnumerableModel<T> : IContinuation
    {
        public ContinuationEnumerableModel()
        {
        }

        public ContinuationEnumerableModel(IContinuationEnumerable<T> continuationEnumerable)
        {
            Items = continuationEnumerable;
            ContinuationToken = continuationEnumerable.ContinuationToken;
        }

        public ContinuationEnumerableModel(IEnumerable<T> items, string? continuationToken)
        {
            Items = items;
            ContinuationToken = continuationToken;
        }

        [Description("The items in the collection.")]
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        [Description("The continuation token, if any. Pass it back to get the rest of the results.")]
        public string? ContinuationToken { get; set; }
    }
}
