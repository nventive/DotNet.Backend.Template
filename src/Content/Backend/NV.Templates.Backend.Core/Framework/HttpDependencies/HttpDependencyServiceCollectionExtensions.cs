using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Core.Framework.HttpDependencies;
using NV.Templates.Backend.Core.Framework.Json;
using Refit;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpDependencyServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpDependencyClient<TClient, TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            RefitSettings? refitSettings = null)
            where TClient : class
            where TOptions : HttpDependencyOptions, new()
        {
            var builder = services.AddHttpClient<TClient, TOptions>(configuration);

            if (refitSettings is null)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true,
                };

                jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                jsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
                jsonSerializerOptions.Converters.Add(new JsonNullableTimeSpanConverter());
                refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(jsonSerializerOptions));
            }

            services.AddSingleton(provider => RequestBuilder.ForType<TClient>(refitSettings));

            builder.AddTypedClient(
                (client, serviceProvider) => RestService.For(
                    client,
                    serviceProvider.GetService<IRequestBuilder<TClient>>()));

            return services;
        }
    }
}
