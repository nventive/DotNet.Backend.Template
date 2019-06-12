using System.Threading.Tasks;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// <see cref="ObjectGraphType"/> for sources of type <see cref="IIdentifiable"/>
    /// that implements the GraphQL Relay Node Interface.
    /// </summary>
    /// <typeparam name="TSourceType">The source type.</typeparam>
    internal abstract class NodeGraphType<TSourceType> : ObjectGraphType<TSourceType>, IGettableById
        where TSourceType : IIdentifiable
    {
        protected NodeGraphType()
        {
            Name = typeof(TSourceType).Name;

            Interface<NodeInterface>();

            Field<NonNullGraphType<IdGraphType>>()
                .Name("id")
                .Description($"The id of the {Name}.")
                .Resolve(x => x.Source.Id);

            Field<NonNullGraphType<StringGraphType>>()
                .Name("nodeType")
                .Description($"The Node type.")
                .Resolve(x => Name);
        }

        /// <summary>
        /// Returns the corresponding <see cref="IIdentifiable"/> by its id.
        /// </summary>
        public abstract Task<TSourceType> GetById(string id);

        async Task<object> IGettableById.GetById(string id)
        {
            return await GetById(id);
        }
    }
}
