using System.Threading.Tasks;
using GraphQL.Authorization;

namespace NV.Templates.Backend.Web.GraphQLApi.Security
{
    /// <summary>
    /// <see cref="IAuthorizationRequirement"/> that checks if there is an authenticated user.
    /// </summary>
    public class RequireUserAuthorizationRequirement : IAuthorizationRequirement
    {
        public Task Authorize(AuthorizationContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (!context.User.Identity.IsAuthenticated)
            {
                context.ReportError("You must be authenticated.");
            }

            return Task.CompletedTask;
        }
    }
}
