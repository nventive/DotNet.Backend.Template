using GraphQL.Types;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.Web.GraphQLApi.General
{
    internal class InfoGraphType : ObjectGraphType<IApplicationInfo>
    {
        public InfoGraphType()
        {
            Name = "Info";
            Description = "Application Information";

            Field(x => x.Name).Description("The application name");
            Field(x => x.Environment).Description("The environment name");
            Field(x => x.Version).Description("The build version of the Core assembly");
        }
    }
}
