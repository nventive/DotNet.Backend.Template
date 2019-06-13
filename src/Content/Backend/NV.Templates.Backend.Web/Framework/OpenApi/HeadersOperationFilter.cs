using System.Collections.Generic;
using NV.Templates.Backend.Web.Framework.Middlewares;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NV.Templates.Backend.Web.Framework.OpenApi
{
    /// <summary>
    /// <see cref="IOperationFilter"/> that adds standard headers.
    /// </summary>
    internal class HeadersOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(Operation operation, OperationFilterContext context)
        {
            foreach (var response in operation.Responses.Values)
            {
                if (response.Headers == null)
                {
                    response.Headers = new Dictionary<string, Header>();
                }

                response.Headers.Add(
                    OperationContextMiddleware.OperationIdHeader,
                    new Header
                    {
                        Description = "Unique Operation Id for this response.",
                        Type = "string",
                    });
            }
        }
    }
}
