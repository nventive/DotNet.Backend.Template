using GraphQL.Types;
using NV.Templates.Backend.Core.Framework;

namespace NV.Templates.Backend.Web.Framework.GraphQL
{
    internal abstract class IdentifiableGraphType<TSourceType> : ObjectGraphType<TSourceType>
        where TSourceType : IIdentifiable
    {
        protected IdentifiableGraphType()
        {
            Name = typeof(TSourceType).Name;

            Field<NonNullGraphType<IdGraphType>>()
                .Name("id")
                .Description($"The id of the {Name}.")
                .Resolve(x => x.Source.Id);
        }
    }
}
