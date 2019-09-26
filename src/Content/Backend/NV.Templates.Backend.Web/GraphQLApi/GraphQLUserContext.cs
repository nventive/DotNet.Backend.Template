#if Auth
using System.Security.Claims;
using GraphQL.Authorization;
#endif

namespace NV.Templates.Backend.Web.GraphQLApi
{
#if Auth
    /// <summary>
    /// Context for GraphQL execution.
    /// </summary>
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        /// <summary>
        /// Gets or sets the authenticated <see cref="ClaimsPrincipal"/>.
        /// </summary>
        public ClaimsPrincipal User { get; set; }
    }
#else
    /// <summary>
    /// Context for GraphQL execution.
    /// </summary>
    public class GraphQLUserContext
    {
    }
#endif
}
