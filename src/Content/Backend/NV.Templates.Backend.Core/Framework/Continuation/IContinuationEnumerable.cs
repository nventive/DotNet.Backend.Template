using System.Collections.Generic;

namespace NV.Templates.Backend.Core.Framework.Continuation
{
    /// <summary>
    /// Represents an <see cref="IEnumerable{T}"/> that has a continuation token.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IContinuationEnumerable<out T> : IEnumerable<T>, IContinuation
    {
    }
}
