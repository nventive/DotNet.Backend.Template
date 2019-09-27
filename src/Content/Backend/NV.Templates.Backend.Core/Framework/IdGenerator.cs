using System;

namespace NV.Templates.Backend.Core.Framework
{
    /// <summary>
    /// Helper class for generating unique identifiers.
    /// </summary>
    public static class IdGenerator
    {
        /// <summary>
        /// Generates a short unique id based on a <see cref="Guid"/>.
        /// </summary>
        public static string Generate() =>
            Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_", StringComparison.Ordinal)
                .Replace("+", "-", StringComparison.Ordinal)
                .Substring(0, 22);
    }
}
