using System;

namespace NV.Templates.Backend.Core.Framework.Exceptions
{
    /// <summary>
    /// A dependency exception represents an error coming from a dependency.
    /// It contains the <see cref="DependencyName"/> and any relevant information.
    /// You may subclass it to include more appropriate context dependending on the dependency.
    /// </summary>
    public class DependencyException : Exception
    {
        public DependencyException()
        {
        }

        public DependencyException(string dependencyName)
        {
            DependencyName = dependencyName;
        }

        public DependencyException(string dependencyName, string message)
            : base(message)
        {
            DependencyName = dependencyName;
        }

        public DependencyException(string dependencyName, Exception inner)
            : base(inner?.Message, inner)
        {
            DependencyName = dependencyName;
        }

        public DependencyException(string dependencyName, string message, Exception inner)
            : base(message, inner)
        {
            DependencyName = dependencyName;
        }

        /// <summary>
        /// Gets the name of the dependency.
        /// </summary>
        public string DependencyName { get; }
    }
}
