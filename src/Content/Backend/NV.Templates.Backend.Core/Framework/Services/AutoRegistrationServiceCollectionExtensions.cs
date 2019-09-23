using System.Reflection;
using NV.Templates.Backend.Core.Framework.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoRegistrationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all services in <paramref name="assembly"/> that are marked with <see cref="RegisterSingletonServiceAttribute"/>,
        /// <see cref="RegisterScopedServiceAttribute"/> or <see cref="RegisterTransientServiceAttribute"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> to scan.</param>
        /// <returns>The <see cref="IServiceCollection"/> with services registered.</returns>
        public static IServiceCollection AutoRegisterServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan =>
            {
                scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.WithAttribute<RegisterSingletonServiceAttribute>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime();

                scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.WithAttribute<RegisterScopedServiceAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();

                scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.WithAttribute<RegisterTransientServiceAttribute>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });

            return services;
        }
    }
}
