using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// GraphQL Interface that matches the GraphQL Relay Node Interface.
    /// </summary>
    internal class NodeInterface : InterfaceGraphType
    {
        public NodeInterface()
        {
            Name = "Node";
            Field<NonNullGraphType<IdGraphType>>("id", "Globally unique node Id");
            Field<NonNullGraphType<StringGraphType>>("nodeType", "The type of the node");
        }
    }
}
