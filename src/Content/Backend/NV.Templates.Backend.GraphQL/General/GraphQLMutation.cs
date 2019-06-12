using NV.Templates.Backend.GraphQL.General;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// The GraphQL mutations for General.
    /// </summary>
    internal partial class GraphQLMutation
    {
        public void GeneralMutations()
        {
            Mutation<SampleEntity, CreateSampleInputGraphType, CreateSamplePayloadGraphType>("createSampleEntity");
        }
    }
}
