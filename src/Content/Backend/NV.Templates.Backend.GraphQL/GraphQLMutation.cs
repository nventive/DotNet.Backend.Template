using NV.Templates.Backend.GraphQL.Framework.GraphQL;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// The GraphQL root query.
    /// </summary>
    internal partial class GraphQLMutation : MutationGraphType
    {
        public GraphQLMutation()
        {
            Description = "Root GraphQL Mutation";

            GeneralMutations();
        }
    }
}
