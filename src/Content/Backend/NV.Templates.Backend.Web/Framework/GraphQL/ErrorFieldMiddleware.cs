using System;
using System.Threading.Tasks;
using FluentValidation;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.ApplicationInsights;
using NV.Templates.Backend.Core.Framework.Exceptions;

namespace NV.Templates.Backend.Web.Framework.GraphQL
{
    /// <summary>
    /// Custom error handling for GraphQL.
    /// </summary>
    public class ErrorFieldMiddleware
    {
        public async Task<object?> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            try
            {
                return await next(context);
            }
            catch (NotFoundException notFoundException)
            {
                var error = HandleException(context, notFoundException);
                error.Data.Add("target", notFoundException.Target);

                return null;
            }
            catch (DependencyException dependencyException)
            {
                var error = HandleException(context, dependencyException);
                error.Data.Add("dependencyName", dependencyException.DependencyName);
                error.Data.Add("underlyingCode", error.Code);
                error.Code = "DEPENDENCY";

                return null;
            }
            catch (ConcurrencyException concurrencyException)
            {
                var error = HandleException(context, concurrencyException);
                error.Data.Add("target", concurrencyException.Target);

                return null;
            }
            catch (ValidationException validationException)
            {
                var error = HandleException(context, validationException);
                error.Data.Add("errors", validationException.Errors);

                return null;
            }
            catch (Exception ex)
            {
                TrackException(context, ex);
                throw;
            }
        }

        private static ExecutionError HandleException(ResolveFieldContext context, Exception ex)
        {
            TrackException(context, ex);

            var error = new ExecutionError($"Error trying to resolve {context.FieldName}.", ex)
            {
                Path = context.Path,
            };
            error.AddLocation(context.FieldAst, context.Document);
            error.Data.Add("message", ex.Message);
            context.Errors.Add(error);

            return error;
        }

        private static void TrackException(ResolveFieldContext context, Exception ex)
        {
            var dependencyResolver = ((Schema)context.Schema).DependencyResolver;
            var telemetryClient = dependencyResolver.Resolve<TelemetryClient>();
            telemetryClient?.TrackException(ex);
        }
    }
}
