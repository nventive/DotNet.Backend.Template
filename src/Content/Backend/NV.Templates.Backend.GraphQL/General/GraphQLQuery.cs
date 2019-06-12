using GraphQL;
using GraphQL.Types;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.GraphQL.General;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// The GraphQL queries for General.
    /// </summary>
    internal partial class GraphQLQuery
    {
        public void GeneralQueries(IDependencyResolver resolver)
        {
            Field<NonNullGraphType<InfoGraphType>>()
                .Name("info")
                .Description("Get application information")
                .Resolve(_ => resolver.Resolve<IApplicationInfo>());

            Field<NonNullGraphType<ListGraphType<SampleNodeGraphType>>>()
                .Name("allSamples")
                .Description("Get all samples entities.")
                .Resolve(_ => new[] { new SampleEntity { Id = "asdf", Name = "asdf" }, new SampleEntity { Id = "fdsa", Name = "fdsa" } });
        }
    }
}
