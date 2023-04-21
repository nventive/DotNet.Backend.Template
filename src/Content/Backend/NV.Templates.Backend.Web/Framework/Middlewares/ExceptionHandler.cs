using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HelpDeskId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NV.Templates.Backend.Web.Framework.Middlewares
{
    internal static class ExceptionHandler
    {
        /// <summary>
        /// The name of the property in <see cref="ProblemDetails"/> that holds the current <see cref="OperationContext.OperationId"/>.
        /// </summary>
        public const string OperationIdProperty = "operationId";

        /// <summary>
        /// The name of the property in <see cref="ProblemDetails"/> that holds the generated <see cref="HelpDeskId"/>.
        /// </summary>
        public const string HelpDeskIdProperty = "helpDeskId";

        /// <summary>
        /// Configure Exception Handling.
        /// </summary>
        public static void ConfigureExceptionHandling(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
            var jsonOptions = app.ApplicationServices.GetRequiredService<IOptions<JsonOptions>>();
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

            app.Run(httpContext => HandleException(httpContext, jsonOptions.Value, env, loggerFactory.CreateLogger(nameof(ExceptionHandler))));
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        public static async Task HandleException(
            HttpContext context,
            JsonOptions jsonOptions,
            IHostEnvironment env,
            ILogger logger)
        {
            var operationContext = context.RequestServices.GetService<IOperationContext>();
            var helpDeskIdGenerator = context.RequestServices.GetService<IHelpDeskIdGenerator>();
            var operationId = operationContext?.Id ?? Activity.Current?.Id;
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var responseModel = CreateErrorModel(exceptionHandlerFeature?.Error);
            responseModel.Extensions[OperationIdProperty] = operationId;
            var helpDeskId = helpDeskIdGenerator?.GenerateReadableId(CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(helpDeskId))
            {
                responseModel.Extensions[HelpDeskIdProperty] = helpDeskId;
                logger.LogError("HelpDeskId: {helpDeskId} - Error: {Error}", helpDeskId, exceptionHandlerFeature?.Error?.ToStringDemystified());
            }

            if (env.IsDevelopment())
            {
                responseModel.Extensions["exceptionType"] = exceptionHandlerFeature?.Error?.GetType()?.ToString();
                responseModel.Extensions["stackTrace"] = exceptionHandlerFeature?.Error?.ToStringDemystified()?.Split(Environment.NewLine);
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = responseModel.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(responseModel, jsonOptions.JsonSerializerOptions), Encoding.UTF8);
        }

        private static ProblemDetails CreateErrorModel(Exception? exception)
        {
            switch (exception)
            {
                case null:
                    return new ProblemDetails
                    {
                        Title = "An unknown error has occurred.",
                        Status = StatusCodes.Status500InternalServerError,
                    };
                case DependencyException dependencyException:
                    return new ProblemDetails
                    {
                        Title = "There has been an error while communicating with a dependency.",
                        Detail = dependencyException.Message,
                        Instance = dependencyException.DependencyName,
                        Status = StatusCodes.Status502BadGateway,
                    };
                case Refit.ApiException refitApiException:
                    return new ProblemDetails
                    {
                        Title = "There has been an error while communicating with a dependency.",
                        Detail = $"{refitApiException.Message} (StatusCode: {(int)refitApiException.StatusCode})",
                        Instance = refitApiException.Uri?.ToString(),
                        Status = StatusCodes.Status502BadGateway,
                    };
                case NotFoundException notFoundException:
                    return new ProblemDetails
                    {
                        Title = "The target was not found.",
                        Detail = notFoundException.Message,
                        Instance = notFoundException.Target,
                        Status = StatusCodes.Status404NotFound,
                    };
                case ConcurrencyException concurrencyException:
                    return new ProblemDetails
                    {
                        Title = "There has been an conflict while trying to update a target.",
                        Detail = concurrencyException.Message,
                        Instance = concurrencyException.Target,
                        Status = StatusCodes.Status409Conflict,
                    };
                case ValidationException validationException:
                    return new ValidationProblemDetails()
                    {
                        Detail = validationException.Message,
                        Status = StatusCodes.Status400BadRequest,
                    };
                default:
                    var model = new ProblemDetails
                    {
                        Title = "Internal server error.",
                        Detail = exception.Message,
                        Status = StatusCodes.Status500InternalServerError,
                    };

                    if (exception is AggregateException aggregateException)
                    {
                        model.Detail = aggregateException
                            .Flatten()
                            .InnerExceptions
                            .Select(e => e.Message)
                            .Aggregate((x, y) => x + "; " + y);
                    }
                    else
                    {
                        if (exception.InnerException != null)
                        {
                            model.Detail = exception.InnerException.Message;
                        }
                    }

                    return model;
            }
        }
    }
}
