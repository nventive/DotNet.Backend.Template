using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL.General
{
    internal class CreateSampleInputGraphType : InputObjectGraphType<SampleEntity>
    {
        public CreateSampleInputGraphType()
        {
            Name = "CreateSampleInput";
            Field(x => x.Name);
        }
    }
}
