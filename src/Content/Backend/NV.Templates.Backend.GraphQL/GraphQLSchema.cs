using GraphQL;
using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// Defines the GraphQL Schema.
    /// </summary>
    public class GraphQLSchema : Schema
    {
        public GraphQLSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }
}
