using GraphQL;
using NV.Templates.Backend.GraphQL.Framework.GraphQL;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// The GraphQL root query.
    /// </summary>
    internal partial class GraphQLQuery : QueryGraphType
    {
        public GraphQLQuery(IDependencyResolver resolver)
        {
            Description = "Root GraphQL Query";

            GeneralQueries(resolver);
        }
    }
}
