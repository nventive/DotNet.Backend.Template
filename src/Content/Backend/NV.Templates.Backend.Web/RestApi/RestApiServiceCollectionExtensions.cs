using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
#if Auth
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
#if Auth
using Microsoft.AspNetCore.Mvc.Authorization;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RestApiServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Rest API services.
        /// </summary>
        internal static IServiceCollection AddRestApi(this IServiceCollection services)
        {
            services
                .AddResponseCaching()
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddControllers(options =>
                {
                    options.Filters.Add(new ResponseCacheAttribute { Location = ResponseCacheLocation.None, NoStore = true });
#if Auth
                    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
#endif
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation();

            return services;
        }
    }
}
