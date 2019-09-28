using AspNetCoreRequestTracing;
using FluentValidation;
using HelpDeskId;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Web;
using NV.Templates.Backend.Web.Framework.Telemetry;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Web project services.
        /// </summary>
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new System.ArgumentNullException(nameof(configuration));
            }

            services.Configure<RequestTracingMiddlewareOptions>(configuration.GetSection(nameof(RequestTracingMiddlewareOptions)));
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ITelemetryInitializer, HttpContextTelemetryInitializer>();
            services.AddValidatorsFromAssemblyContaining<Startup>();
            services.AddSingleton<IHelpDeskIdGenerator, HelpDeskIdGenerator>();

            services
                .AddHttpContextAccessor()
                .AddCors()
                .AddRouting();

            return services;
        }
    }
}
