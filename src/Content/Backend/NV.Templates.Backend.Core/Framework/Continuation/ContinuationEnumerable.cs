using System.Linq;

namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// A decorator for <see cref="IEnumerable{T}"/> that provides a <see cref="IContinuationEnumerable{T}"/> implementation.
    /// </summary>
    public static class ContinuationEnumerable
    {
        /// <summary>
        /// Returns an empty <see cref="IContinuationEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        public static IContinuationEnumerable<T> Empty<T>() => new ContinuationEnumerable<T>(Enumerable.Empty<T>());
    }
}
