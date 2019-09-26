using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Web.Framework.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services and configuration related to Authentication.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new System.ArgumentNullException(nameof(configuration));
            }

            var authenticationOptions = configuration.GetSection(nameof(AuthenticationOptions)).Get<AuthenticationOptions>();

            Validator.ValidateObject(authenticationOptions, new ValidationContext(authenticationOptions), true);

            services.Configure<AuthenticationOptions>(configuration.GetSection(nameof(AuthenticationOptions)));
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = authenticationOptions.JwtAudience;
                    options.Authority = authenticationOptions.JwtAuthority;
                });

            return services;
        }
    }
}
