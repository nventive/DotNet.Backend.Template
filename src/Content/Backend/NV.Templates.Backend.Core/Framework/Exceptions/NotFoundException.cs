using System;

namespace NV.Templates.Backend.Core.Framework.Exceptions
{
    /// <summary>
    /// Represents an error when a requested item could not be found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string target)
        {
            Target = target;
        }

        public NotFoundException(string target, string message)
            : base(message)
        {
            Target = target;
        }

        public NotFoundException(string target, Exception inner)
            : base(inner.Message, inner)
        {
            Target = target;
        }

        public NotFoundException(string target, string message, Exception inner)
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
