using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NV.Templates.Backend.Core.General;

namespace NV.Templates.Backend.GraphQL.Framework.Middlewares
{
    /// <summary>
    /// This middleware is responsible for setting the current <see cref="IOperationContext.OperationId"/>
    /// from <see cref="Activity.Current.Id"/>
    /// </summary>
    internal class OperationContextMiddleware
    {
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
        public Task Invoke(HttpContext context)
        {
            var operationContext = context.RequestServices.GetRequiredService<IOperationContext>();
            operationContext.OperationId = Activity.Current.Id;
            operationContext.Timestamp = _clock.GetCurrentInstant();
            return _next(context);
        }
    }
}
