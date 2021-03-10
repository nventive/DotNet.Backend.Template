using HttpTracing;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core.Configuration;
using NV.Templates.Backend.Core.General;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Core services.
        /// </summary>
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.BindOptionsToConfigurationAndValidate<BackendOptions>(configuration);

            services.AddHttpTracingToAllHttpClients((sp, builder) =>
            {
                return new HttpMessageHandlerTracingConfiguration
                {
                    BufferRequests = true,
                };
            });

            services.AutoRegisterServicesFromAssemblyContaining<IApplicationInfo>();

            services.AddHealthChecks();

            return services;
        }
    }
}
