using FluentValidation;
using Microsoft.Extensions.Hosting;
using NV.Templates.Backend.Core.Framework.Services;
using NV.Templates.Backend.Core.Framework.Validation;
using NV.Templates.Backend.Core.General;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Core services.
        /// </summary>
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IApplicationInfo>();
            services.AutoRegisterServicesFromAssembly(typeof(CoreServiceCollectionExtensions).Assembly);

            services
                .AddHealthChecks();

            return services;
        }
    }
}
