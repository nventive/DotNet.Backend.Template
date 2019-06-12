using System;
using System.Threading.Tasks;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework;
using NV.Templates.Backend.GraphQL.Framework.GraphQL;

namespace NV.Templates.Backend.GraphQL.General
{
    internal class CreateSamplePayloadGraphType : MutationPayloadGraphType<SampleEntity, SampleEntity>
    {
        public CreateSamplePayloadGraphType()
        {
            Name = "CreateSamplePayload";
            Field(x => x.Id);
            Field(x => x.Name);
        }

        protected override async Task<SampleEntity> Mutate(SampleEntity input, ResolveFieldContext<object> context)
        {
            return new SampleEntity { Id = IdGenerator.Generate(), Name = input.Name };
        }
    }
}
