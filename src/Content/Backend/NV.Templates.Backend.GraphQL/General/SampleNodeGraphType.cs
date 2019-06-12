using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NV.Templates.Backend.Core.Framework;
using NV.Templates.Backend.GraphQL.Framework.GraphQL;

namespace NV.Templates.Backend.GraphQL.General
{
    internal class SampleNodeGraphType : NodeGraphType<SampleEntity>
    {
        public SampleNodeGraphType()
        {
            Field(x => x.Name);
        }

        public override async Task<SampleEntity> GetById(string localId)
        {
            // Fetch the entity by id.
            return new SampleEntity { Id = localId, Name = localId };
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Sample code.")]
    internal class SampleEntity : NamedEntity
    {
    }
}
