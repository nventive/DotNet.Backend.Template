using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace NV.Templates.Backend.Web.GraphQLApi
{
    /// <summary>
    /// <see cref="IUserContextBuilder"/> that builds <see cref="GraphQLUserContext"/>.
    /// </summary>
    public class GraphQLUserContextBuilder : IUserContextBuilder
    {
#if Auth
        public Task<object> BuildUserContext(HttpContext httpContext)
            => Task.FromResult<object>(new GraphQLUserContext { User = httpContext?.User });
#else
        public Task<object> BuildUserContext(HttpContext httpContext)
            => Task.FromResult<object>(new GraphQLUserContext());
#endif
    }
}
