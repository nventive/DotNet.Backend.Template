using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// <see cref="IUserContextBuilder"/> that builds <see cref="GraphQLUserContext"/>.
    /// </summary>
    public class GraphQLUserContextBuilder : IUserContextBuilder
    {
        public Task<object> BuildUserContext(HttpContext httpContext)
            => Task.FromResult<object>(new GraphQLUserContext());
    }
}
