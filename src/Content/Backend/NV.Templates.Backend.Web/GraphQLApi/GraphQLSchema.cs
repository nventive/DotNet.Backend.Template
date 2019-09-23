using GraphQL;
using GraphQL.Types;

namespace NV.Templates.Backend.Web.GraphQLApi
{
    /// <summary>
    /// Defines the GraphQL Schema.
    /// </summary>
    internal class GraphQLSchema : Schema
    {
        public GraphQLSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            // Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }
}
