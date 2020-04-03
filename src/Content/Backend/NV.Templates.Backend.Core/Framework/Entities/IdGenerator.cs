using System;
using System.Globalization;

namespace NV.Templates.Backend.Core.Framework.Entities
{
    /// <summary>
    /// Helper class for generating unique identifiers.
    /// </summary>
    public static class IdGenerator
    {
        /// <summary>
        /// Generates a short unique id.
        /// </summary>
        public static string Generate() => Guid.NewGuid().ToString("n", CultureInfo.InvariantCulture);
    }
}
