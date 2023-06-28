using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NV.Templates.Backend.Web.Framework.Middlewares
{
    /// <summary>
    /// This middleware is responsible for setting the current <see cref="IOperationContext"/> values.
    /// </summary>
    internal class OperationContextMiddleware
    {
        /// <summary>
        /// Gets the Response Header for the current <see cref="OperationContext.OperationId"/>.
        /// </summary>
        public const string OperationIdHeader = "X-OperationId";

        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContextMiddleware"/> class.
        /// </summary>
        public OperationContextMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Invoked by ASP.NET.
        /// </summary>
        public Task Invoke(HttpContext context, IOperationContext operationContext)
        {
            operationContext.Id = Activity.Current?.RootId ?? string.Empty;
            operationContext.Timestamp = DateTimeOffset.UtcNow;
            operationContext.User = context.User;

            context.Response.Headers.Append(OperationIdHeader, operationContext.Id);
            return _next(context);
        }
    }
}
