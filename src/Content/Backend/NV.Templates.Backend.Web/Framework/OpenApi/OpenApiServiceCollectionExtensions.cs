using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSwag;
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
        public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
#if Auth
            services.BindOptionsToConfigurationAndValidate<OpenApiSecurityScheme>(configuration);
#endif
            var initProvider = services.BuildServiceProvider();
            var serviceInfo = initProvider.GetRequiredService<IApplicationInfo>();
            var apiVersionDescriptionProvider = initProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(x => x.GroupName))
            {
                services.AddOpenApiDocument((document, sp) =>
                {
                    var appInfo = sp.GetRequiredService<IApplicationInfo>();

                    document.DocumentName = apiVersionDescription.GroupName;
                    document.Title = $"{serviceInfo.Name} ({serviceInfo.Environment})";
                    document.Version = serviceInfo.Version;
                    document.Description = $"<i>For 3rd party licenses see <a href='{AttributionsHandler.Path}'>attributions</a>.</i>";
                    document.ApiGroupNames = new[] { apiVersionDescription.GroupName };

                    // TODO: This will need updating when NSwag support System.Text.Json properly.
                    document.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                        {
                            NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy(),
                        },
                        Converters = new List<Newtonsoft.Json.JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
                    };

                    document.SchemaNameGenerator = new CustomSchemaNameGenerator();

                    document.DocumentProcessors.Add(new HealthChecksDocumentProcessor());
                    document.OperationProcessors.Add(new CommonHeadersOperationProcessor());

#if Auth
                    var openApiSecurityScheme = sp.GetRequiredService<IOptions<OpenApiSecurityScheme>>().Value;
                    document.AddSecurity("OAuth2", openApiSecurityScheme);
#endif
                });
            }

            return services;
        }
    }
}
