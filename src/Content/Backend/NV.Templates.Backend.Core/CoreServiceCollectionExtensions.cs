using Microsoft.Extensions.Configuration;
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
            // services.BindOptionsToConfigurationAndValidate<>(configuration);

            services.AutoRegisterServicesFromAssemblyContaining<IApplicationInfo>();

            services.AddHealthChecks();

            return services;
        }
    }
}
