using System.Threading.Tasks;
using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// Base class for Graph types that handles mutations and return values.
    /// </summary>
    /// <typeparam name="TMutationInput">The input type (not the graph type, but the type it resolves to.)</typeparam>
    /// <typeparam name="TMutationResult">The result type (that this type will map to.)</typeparam>
    internal abstract class MutationPayloadGraphType<TMutationInput, TMutationResult> : ObjectGraphType<TMutationResult>, IMutable<TMutationInput>
    {
        async Task<object> IMutable<TMutationInput>.Mutate(TMutationInput input, ResolveFieldContext<object> context) =>
            await Mutate(input, context);

        protected abstract Task<TMutationResult> Mutate(TMutationInput input, ResolveFieldContext<object> context);
    }
}
