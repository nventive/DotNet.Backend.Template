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
        public GraphQLUserContext(ClaimsPrincipal user)
        {
            User = user;
        }

        /// <summary>
        /// Gets the authenticated <see cref="ClaimsPrincipal"/>.
        /// </summary>
        public ClaimsPrincipal User { get; }
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
