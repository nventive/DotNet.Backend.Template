using System.Threading.Tasks;
using GraphQL.Types;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// Defines a <see cref="Mutate(TMutationInput, ResolveFieldContext{object})"/> method.
    /// </summary>
    /// <typeparam name="TMutationInput">The mutation input type.</typeparam>
    internal interface IMutable<TMutationInput>
    {
        Task<object> Mutate(TMutationInput input, ResolveFieldContext<object> context);
    }
}
