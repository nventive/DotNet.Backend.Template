using System.Threading.Tasks;
using NV.Templates.Backend.Core.Framework;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    internal interface IGettableById
    {
        /// <summary>
        /// Returns a <see cref="IIdentifiable"/> by its id.
        /// </summary>
        Task<object> GetById(string id);
    }
}
