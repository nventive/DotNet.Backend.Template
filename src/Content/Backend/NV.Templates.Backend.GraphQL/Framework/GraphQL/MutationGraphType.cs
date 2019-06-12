using GraphQL.Builders;
using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// Base class for root mutation.
    /// </summary>
    internal abstract class MutationGraphType : ObjectGraphType
    {
        public const string InputFieldName = "input";

        protected MutationGraphType()
        {
            Name = "Mutation";
        }

        /// <summary>
        /// Register a mutation with graph types.
        /// </summary>
        /// <typeparam name="TMutationInput">The type for input.</typeparam>
        /// <typeparam name="TMutationInputGraphType">The graphql type for input.</typeparam>
        /// <typeparam name="TMutationPayloadGraphType">The graphql type returned.</typeparam>
        protected FieldBuilder<object, object> Mutation<
            TMutationInput,
            TMutationInputGraphType,
            TMutationPayloadGraphType>(
            string name,
            string inputDescription = null)
            where TMutationInputGraphType : InputObjectGraphType<TMutationInput>
            where TMutationPayloadGraphType : GraphType, IMutable<TMutationInput>
        {
            return Field<TMutationPayloadGraphType>()
                .Name(name)
                .Argument<NonNullGraphType<TMutationInputGraphType>>(InputFieldName, inputDescription ?? "The mutation argument")
                .ResolveAsync(async context =>
                {
                    var input = context.GetArgument<TMutationInput>(InputFieldName);

                    return await ((TMutationPayloadGraphType)context.ReturnType).Mutate(input, context);
                });
        }
    }
}
