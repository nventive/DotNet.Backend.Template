using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.Web.Framework.Middlewares;
using NV.Templates.Backend.Web.Framework.OpenApi;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class OpenApiServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Open Api services.
        /// </summary>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            var initProvider = services.BuildServiceProvider();
            var applicationInfo = initProvider.GetRequiredService<IApplicationInfo>();
            var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(x => x.GroupName))
            {
                services.AddOpenApiDocument(document =>
                {
                    document.DocumentName = apiVersionDescription.GroupName;
                    document.Title = $"{applicationInfo.Name} ({applicationInfo.Environment})";
                    document.Version = applicationInfo.Version;
                    document.Description = $"<i>For 3rd party licenses see <a href='{AttributionsHandler.Path}'>attributions</a>.</i>";
                    document.ApiGroupNames = new[] { apiVersionDescription.GroupName };

                    document.DocumentProcessors.Add(new HealthChecksDocumentProcessor());
                    document.OperationProcessors.Add(new CommonHeadersOperationProcessor());
                    document.OperationProcessors.Add(new CommonErrorsOperationProcessor());
                });
            }

            return services;
        }
    }
}
