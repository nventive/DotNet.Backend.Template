using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NV.Templates.Backend.RestApi.Framework.Middlewares;

namespace NV.Templates.Backend.RestApi.Framework.OpenApi
{
    /// <summary>
    /// <see cref="IOperationProcessor"/> implementation that Adds common headers to all operations.
    /// </summary>
    internal class CommonHeadersOperationProcessor : IOperationProcessor
    {
        /// <inheritdoc />
        public bool Process(OperationProcessorContext context)
        {
            foreach (var response in context.OperationDescription.Operation.Responses.Values)
            {
                response.Headers.Add(
                    OperationContextMiddleware.OperationIdHeader,
                    new JsonSchema
                    {
                        Type = JsonObjectType.String,
                        Description = "Unique Operation Id for this response.",
                    });
            }

            return true;
        }
    }
}
