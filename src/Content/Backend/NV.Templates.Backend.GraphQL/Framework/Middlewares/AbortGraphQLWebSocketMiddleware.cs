using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NV.Templates.Backend.GraphQL.Framework.Middlewares
{
    /// <summary>
    /// This middleware aborts request processing for any GraphQL WebSocket connection.
    /// The GraphiQL web UI and some GraphQL clients are quite agressive for those connections
    /// and tend to pollute request processing.
    /// </summary>
    internal class AbortGraphQLWebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortGraphQLWebSocketMiddleware"/> class.
        /// </summary>
        public AbortGraphQLWebSocketMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Invoked by ASP.NET.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.WebSocketRequestedProtocols.Contains("graphql-ws"))
            {
                context.Abort();
                return;
            }

            await _next(context);
        }
    }
}
