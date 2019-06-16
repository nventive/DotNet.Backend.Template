using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NodaTime;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.GraphQL.Framework.Middlewares
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
        private readonly IClock _clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContextMiddleware"/> class.
        /// </summary>
        public OperationContextMiddleware(RequestDelegate next, IClock clock)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        /// <summary>
        /// Invoked by ASP.NET.
        /// </summary>
        public Task Invoke(HttpContext context, IOperationContext operationContext)
        {
            operationContext.OperationId = Activity.Current.RootId;
            operationContext.Timestamp = _clock.GetCurrentInstant();

            context.Response.Headers.Add(OperationIdHeader, operationContext.OperationId);
            return _next(context);
        }
    }
}
