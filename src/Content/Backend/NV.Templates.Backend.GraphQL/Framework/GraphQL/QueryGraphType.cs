using System.Threading.Tasks;
using GraphQL.Types;
using NV.Templates.Backend.Core.Framework.Exceptions;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// Base class for root queries that provides a node interface to retrieve
    /// objects by ids.
    /// </summary>
    internal abstract class QueryGraphType : ObjectGraphType
    {
        protected QueryGraphType()
        {
            Name = "Query";

            Field<NodeInterface>()
                .Name("node")
                .Description("Fetches an object given its global Id")
                .Argument<NonNullGraphType<IdGraphType>>("id", "The Id of the node")
                .Argument<NonNullGraphType<StringGraphType>>("nodeType", "The node type")
                .ResolveAsync(ResolveObjectFromIdAndNodeType);
        }

        private Task<object> ResolveObjectFromIdAndNodeType(ResolveFieldContext<object> context)
        {
            var idArg = context.GetArgument<string>("id");
            var nodeTypeArg = context.GetArgument<string>("nodeType");
            if (!string.IsNullOrEmpty(idArg) && !string.IsNullOrEmpty(nodeTypeArg))
            {
                if (context.Schema.FindType(nodeTypeArg) is IGettableById node)
                {
                    return node.GetById(idArg);
                }
            }

            throw new NotFoundException($"{nodeTypeArg}/{idArg}");
        }
    }
}
