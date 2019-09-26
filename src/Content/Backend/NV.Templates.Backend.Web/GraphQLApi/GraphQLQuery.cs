using GraphQL;
using GraphQL.Types;

namespace NV.Templates.Backend.Web.GraphQLApi
{
    /// <summary>
    /// The GraphQL root query.
    /// </summary>
    internal partial class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(IDependencyResolver resolver)
        {
            Description = "Root GraphQL Query";

            GeneralQueries(resolver);
        }
    }
}
