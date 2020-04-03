using System;
using AspNetCoreRequestTracing;
using HelpDeskId;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Web.Framework.Telemetry;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Web project services.
        /// </summary>
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.BindOptionsToConfigurationAndValidate<RequestTracingMiddlewareOptions>(configuration);

            services
                .AddApplicationInsightsTelemetry()
                .AddSingleton<ITelemetryInitializer, HttpContextTelemetryInitializer>();

            services.AddSingleton<IHelpDeskIdGenerator, HelpDeskIdGenerator>();

            services
                .AddCors()
                .AddResponseCaching()
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                });

            return services;
        }
    }
}
