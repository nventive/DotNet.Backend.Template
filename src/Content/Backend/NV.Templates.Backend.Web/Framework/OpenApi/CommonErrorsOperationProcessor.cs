using System;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NV.Templates.Backend.Web.Framework.Middlewares;

namespace NV.Templates.Backend.Web.Framework.OpenApi
{
    /// <summary>
    /// <see cref="IOperationProcessor"/> implementation that Adds common errors to all operations.
    /// </summary>
    internal class CommonErrorsOperationProcessor : IOperationProcessor
    {
        /// <inheritdoc />
        public bool Process(OperationProcessorContext context)
        {
            var operationDetailsSchema = context.SchemaGenerator.Generate(typeof(ProblemDetails));
            operationDetailsSchema.Properties.Add(
                ExceptionHandler.OperationIdProperty,
                new JsonSchemaProperty
                {
                    Type = JsonObjectType.String,
                    Description = "The unique, technical Operation Id associated with the request.",
                });
            operationDetailsSchema.Properties.Add(
                ExceptionHandler.HelpDeskIdProperty,
                new JsonSchemaProperty
                {
                    Type = JsonObjectType.String,
                    Description = "A human-readable identifier associated with this error that can be correlated with the request.",
                });

            if (!context.OperationDescription.Operation.Responses.ContainsKey("5XX"))
            {
                context.OperationDescription.Operation.Responses.Add(
                    "5XX",
                    new OpenApiResponse
                    {
                        Description = "A server error has occured.",
                        Schema = operationDetailsSchema,
                    });
            }

            return true;
        }
    }
}
