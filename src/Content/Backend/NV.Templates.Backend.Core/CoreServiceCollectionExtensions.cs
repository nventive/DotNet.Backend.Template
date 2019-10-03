using System;
using FluentValidation;
#if EFCore
using Microsoft.EntityFrameworkCore;
#endif
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core;
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
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddValidatorsFromAssemblyContaining<IApplicationInfo>();
            services.AutoRegisterServicesFromAssembly(typeof(CoreServiceCollectionExtensions).Assembly);

#if EFCore
            services
                .AddDbContext<CoreDbContext>(options =>
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString(nameof(CoreDbContext)),
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
                });

#endif
            services
                .AddHealthChecks();

            return services;
        }
    }
}
