using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// A decorator for <see cref="IEnumerable{T}"/> that provides a <see cref="IContinuationEnumerable{T}"/> implementation.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public class ContinuationEnumerableDecorator<T> : IContinuationEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuationEnumerableDecorator{T}"/> class.
        /// </summary>
        /// <param name="inner">The original <see cref="IEnumerable{T}"/>.</param>
        /// <param name="continuationToken">The continuation token if any.</param>
        public ContinuationEnumerableDecorator(IEnumerable<T> inner, string continuationToken = null)
        {
            _inner = inner ?? Enumerable.Empty<T>();
            ContinuationToken = continuationToken;
        }

        /// <inheritdoc/>
        public string ContinuationToken { get; }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
