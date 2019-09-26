using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace NV.Templates.Backend.Web.Framework.Telemetry
{
    /// <summary>
    /// <see cref="ITelemetryInitializer"/> implementation that sets telemetry information
    /// from the current <see cref="HttpContext"/>.
    /// </summary>
    internal class HttpContextTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Gets the name of the session id header.
        /// </summary>
        public const string SessionIdHeader = "X-SessionId";

        /// <summary>
        /// Gets the name of the device id header.
        /// </summary>
        public const string DeviceIdHeader = "X-DeviceId";

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContextTelemetryInitializer"/> class.
        /// </summary>
        public HttpContextTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public void Initialize(ITelemetry telemetry)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    telemetry.Context.User.Id = httpContext.User.Identity.Name;
                    telemetry.Context.User.AuthenticatedUserId = httpContext.User.Identity.Name;
                }

                var userAgentHeader = httpContext.Request?.Headers["User-Agent"];
                if (!string.IsNullOrEmpty(userAgentHeader))
                {
                    telemetry.Context.User.UserAgent = userAgentHeader;
                }

                var sessionIdHeader = httpContext.Request?.Headers["X-SessionId"];
                if (!string.IsNullOrEmpty(sessionIdHeader))
                {
                    telemetry.Context.Session.Id = sessionIdHeader;
                }

                var deviceIdHeader = httpContext.Request?.Headers["X-DeviceId"];
                if (!string.IsNullOrEmpty(deviceIdHeader))
                {
                    telemetry.Context.Device.Id = deviceIdHeader;
                }
            }
        }
    }
}
