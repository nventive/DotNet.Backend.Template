using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core.Framework.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class RegistrationServiceCollectionExtensions
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

        /// <summary>
        /// Registers all services in the assembly containing <typeparamref name="T"/> that are marked with
        /// <see cref="RegisterSingletonServiceAttribute"/>, <see cref="RegisterScopedServiceAttribute"/> or
        /// <see cref="RegisterTransientServiceAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type contained in the assembly to scan.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> with services registered.</returns>
        public static IServiceCollection AutoRegisterServicesFromAssemblyContaining<T>(this IServiceCollection services)
            => services.AutoRegisterServicesFromAssembly(typeof(T).Assembly);

        /// <summary>
        /// Registers <typeparamref name="T"/> as an option bound to the <paramref name="configuration"/>
        /// using the typename as key (minus the -Options prefix).
        /// </summary>
        /// <typeparam name="T">The type of options to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> with services registered.</returns>
        public static IServiceCollection BindOptionsToConfigurationAndValidate<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services
                .AddOptions<T>()
                .Bind(configuration.GetSection(DefaultOptionsName<T>()))
                .ValidateDataAnnotations();

            return services;
        }

        /// <summary>
        /// Gets the default name for options of type <paramref name="optionsType"/>.
        /// Removes the -Options prefix if any.
        /// </summary>
        public static string DefaultOptionsName(Type optionsType) => Regex.Replace(optionsType.Name, @"Options$", string.Empty);

        /// <summary>
        /// Gets the default name for options of type <typeparamref name="T"/>.
        /// Removes the -Options prefix if any.
        /// </summary>
        /// <typeparam name="T">The Options type.</typeparam>
        public static string DefaultOptionsName<T>() => DefaultOptionsName(typeof(T));
    }
}
