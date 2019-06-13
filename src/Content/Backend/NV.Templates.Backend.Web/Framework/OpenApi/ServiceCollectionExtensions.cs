using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.Web.Framework.Middlewares;
using NV.Templates.Backend.Web.Framework.OpenApi;
using Swashbuckle.AspNetCore.Swagger;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Core services.
        /// </summary>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            var initProvider = services.BuildServiceProvider();
            var applicationInfo = initProvider.GetRequiredService<IApplicationInfo>();
            var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            var jsonOptions = initProvider.GetRequiredService<IOptions<MvcJsonOptions>>().Value;

            services.AddSwaggerGen(options =>
            {
                foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    var info = new Info
                    {
                        Title = $"{applicationInfo.Name} ({applicationInfo.Environment})",
                        Version = applicationInfo.Version,
                        Description = $"<i>For 3rd party licenses see <a href='{AttributionsHandler.Path}'>attributions</a>.</i>",
                    };
                    options.SwaggerDoc(apiVersionDescription.GroupName, info);
                }

                options.OperationFilter<HeadersOperationFilter>();
                options.DocumentFilter<HealthChecksDocumentFilter>();
                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
                options.AddFluentValidationRules();
                options.ConfigureForNodaTime(jsonOptions.SerializerSettings);
            });

            return services;
        }
    }
}
