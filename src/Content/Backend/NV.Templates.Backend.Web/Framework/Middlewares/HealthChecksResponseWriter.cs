using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace NV.Templates.Backend.Web.Framework.Middlewares
{
    internal static class HealthChecksResponseWriter
    {
        /// <summary>
        /// The standard endpoint for health checks.
        /// </summary>
        public const string HealthChecksEndpoint = "/api/health";

        /// <summary>
        /// Writes health checks results.
        /// </summary>
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var jsonOptions = httpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value;
            return httpContext.Response.WriteAsync(JsonSerializer.Serialize(result, jsonOptions.JsonSerializerOptions));
        }
    }
}
