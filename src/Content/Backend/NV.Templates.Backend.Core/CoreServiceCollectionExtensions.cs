using HttpTracing;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core.Configuration;

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

            services.AutoRegisterServicesFromAssembly();

            services.AddHealthChecks();

            return services;
        }
    }
}
