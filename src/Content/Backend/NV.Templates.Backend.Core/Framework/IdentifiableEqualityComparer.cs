using System;
using System.Collections.Generic;

namespace NV.Templates.Backend.Core.Framework
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> implementation for <see cref="IIdentifiable"/>
    /// that only compares <see cref="IIdentifiable.Id"/> properties.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class IdentifiableEqualityComparer<T> : IEqualityComparer<T>
        where T : IIdentifiable
    {
        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return string.Equals(x.Id, y.Id, StringComparison.Ordinal);
        }

        public int GetHashCode(T obj) => (obj.Id.GetHashCode(StringComparison.Ordinal) ^ typeof(T).GetHashCode()).GetHashCode();
    }
}
