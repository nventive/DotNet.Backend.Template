using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace NV.Templates.Backend.Web.Framework.Middlewares
{
    internal static class HealthChecksResponseWriter
    {
        /// <summary>
        /// Writes health checks results.
        /// </summary>
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var mvcJsonOptions = httpContext.RequestServices.GetRequiredService<IOptions<MvcJsonOptions>>();
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result, Formatting.Indented, mvcJsonOptions.Value.SerializerSettings));
        }
    }
}
