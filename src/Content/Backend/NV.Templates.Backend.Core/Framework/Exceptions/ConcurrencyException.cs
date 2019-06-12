using System;

namespace NV.Templates.Backend.Core.Framework.Exceptions
{
    /// <summary>
    /// Represents an exception that indicates a concurrency conflict.
    /// </summary>
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string target)
        {
            Target = target;
        }

        public ConcurrencyException(string target, string message)
            : base(message)
        {
            Target = target;
        }

        public ConcurrencyException(string target, Exception inner)
            : base(inner.Message, inner)
        {
            Target = target;
        }

        public ConcurrencyException(string target, string message, Exception inner)
            : base(message, inner)
        {
            Target = target;
        }

        /// <summary>
        /// Gets information about the expected target (such as id).
        /// </summary>
        public string Target { get; }
    }
}
