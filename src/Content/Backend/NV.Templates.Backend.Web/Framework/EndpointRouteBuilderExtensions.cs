using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Routing
{
    /// <summary>
    /// <see cref="IEndpointRouteBuilder"/> extensions.
    /// </summary>
    internal static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps the <see cref="HealthChecksResponseWriter.HealthChecksEndpoint"/>.
        /// </summary>
        public static IEndpointRouteBuilder MapCommonHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks(
                    HealthChecksResponseWriter.HealthChecksEndpoint,
                    new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });

            return endpoints;
        }

        /// <summary>
        /// Maps a redirect from <paramref name="pattern"/> to the <paramref name="target"/> on GET requests.
        /// </summary>
        public static IEndpointRouteBuilder MapGetRedirect(this IEndpointRouteBuilder endpoints, string pattern, string target)
        {
            endpoints.MapGet(
                pattern,
                async ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status307TemporaryRedirect;
                    ctx.Response.Headers["Location"] = target;
                });

            return endpoints;
        }

        /// <summary>
        /// Maps root requests("/") to OpenApi UI ("/swagger").
        /// </summary>
        public static IEndpointRouteBuilder MapRootToOpenApiUi(this IEndpointRouteBuilder endpoints)
        {
            var conf = endpoints.ServiceProvider.GetService(typeof(IOptions<BackendOptions>)) as IOptions<BackendOptions>;

            if (!(conf?.Value.EnableSwagger ?? false))
            {
                // If Swagger is not enabled, calls to root path are routed to ping controller to
                // prevent AppService AlwaysOn feature to generate failed requests
                return endpoints.MapGetRedirect("/", "/ping");
            }

            return endpoints.MapGetRedirect("/", "/swagger");
        }
    }
}
