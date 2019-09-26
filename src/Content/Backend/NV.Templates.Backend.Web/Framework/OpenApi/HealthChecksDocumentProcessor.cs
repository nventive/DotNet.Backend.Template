using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace NV.Templates.Backend.Web.Framework.OpenApi
{
    internal class HealthChecksDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            var healthReportSchema = context.SchemaGenerator.Generate(typeof(HealthReport), context.SchemaResolver);

            var getHealthOperation = new OpenApiOperation
            {
                OperationId = "GetHealth",
                Produces = new List<string> { "application/json" },
                Summary = "Performs health checks",
                Tags = new List<string> { "General" },
            };

            getHealthOperation.Responses.Add(
                StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture),
                new OpenApiResponse { Description = "Application is running OK", Schema = healthReportSchema });

            getHealthOperation.Responses.Add(
                StatusCodes.Status503ServiceUnavailable.ToString(CultureInfo.InvariantCulture),
                new OpenApiResponse { Description = "At least one health check reported an issue", Schema = healthReportSchema });

            var healthPathItem = new OpenApiPathItem
            {
                { "GET", getHealthOperation },
            };

            context.Document.Paths.Add("/api/health", healthPathItem);
        }
    }
}
