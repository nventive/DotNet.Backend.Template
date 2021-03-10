using System.Text.Json.Serialization;
#if Auth
using Microsoft.AspNetCore.Authorization;
#endif
using Microsoft.AspNetCore.Mvc;
#if Auth
using Microsoft.AspNetCore.Mvc.Authorization;
#endif
using NV.Templates.Backend.Core.Framework.Json;

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
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VV";
                    options.SubstituteApiVersionInUrl = true;
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
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonNullableTimeSpanConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            return services;
        }
    }
}
