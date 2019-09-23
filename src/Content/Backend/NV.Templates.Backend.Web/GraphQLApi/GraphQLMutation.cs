using System;
using System.Threading.Tasks;
using GraphQL.Builders;
using GraphQL.Types;

namespace NV.Templates.Backend.Web.GraphQLApi
{
    /// <summary>
    /// The GraphQL root query.
    /// </summary>
    internal partial class GraphQLMutation : ObjectGraphType
    {
        public GraphQLMutation()
        {
            Description = "Root GraphQL Mutation";
        }

        private FieldBuilder<object, object> Mutation<TReturnGraphType, TInputGraphType, TInput>(
            string name,
            Func<ResolveFieldContext<object>, TInput, Task<object>> resolveAsync)
            where TReturnGraphType : GraphType
            where TInputGraphType : InputObjectGraphType<TInput>
        {
            return Field<TReturnGraphType>()
                .Name(name)
                .Argument<NonNullGraphType<TInputGraphType>>("input", "arguments")
                .ResolveAsync(context =>
                {
                    var input = context.GetArgument<TInput>("input");

                    return resolveAsync(context, input);
                });
        }
    }
}
