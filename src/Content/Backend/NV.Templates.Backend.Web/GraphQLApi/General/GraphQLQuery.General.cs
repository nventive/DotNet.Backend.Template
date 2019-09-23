using GraphQL;
using GraphQL.Types;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.Web.GraphQLApi.General;

namespace NV.Templates.Backend.Web.GraphQLApi
{
    internal partial class GraphQLQuery
    {
        public void GeneralQueries(IDependencyResolver resolver)
        {
            Field<NonNullGraphType<InfoGraphType>>()
                .Name("info")
                .Description("Get application information")
                .Resolve(_ => resolver.Resolve<IApplicationInfo>());
        }
    }
}
