using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NV.Templates.Backend.Web.Framework.OpenApi
{
    /// <summary>
    /// <see cref="IDocumentFilter"/> that adds the health check middleware endpoint.
    /// </summary>
    internal class HealthChecksDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var healthReportSchema = context.SchemaRegistry.GetOrRegister(typeof(HealthReport));

            var healthPathItem = new PathItem
            {
                Get = new Operation
                {
                    Consumes = new List<string>(),
                    OperationId = "GetHealth",
                    Parameters = new List<IParameter>(),
                    Produces = new List<string> { "application/json" },
                    Responses = new Dictionary<string, Response>
                    {
                        {
                            StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture),
                            new Response { Description = "Application is running OK", Schema = healthReportSchema }
                        },
                        {
                            StatusCodes.Status503ServiceUnavailable.ToString(CultureInfo.InvariantCulture),
                            new Response { Description = "At least one health check reported an issue", Schema = healthReportSchema }
                        },
                    },
                    Summary = "Performs health checks",
                    Tags = new List<string> { "General" },
                },
            };

            if (swaggerDoc.Paths == null)
            {
                swaggerDoc.Paths = new Dictionary<string, PathItem>();
            }

            swaggerDoc.Paths.Add("/health", healthPathItem);
        }
    }
}
