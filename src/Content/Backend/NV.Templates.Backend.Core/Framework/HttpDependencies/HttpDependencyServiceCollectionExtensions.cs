using System;
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
        /// <summary>
        /// Adds a Refit client for <see cref="TClient"/> and automatically applies the
        /// options for <typeparamref name="TOptions"/>.
        /// <see cref="HttpClientBuilderExtensions.ConfigureWithOptions"/> for more information.
        /// </summary>
        /// <typeparam name="TClient">The Refit Client interface.</typeparam>
        /// <typeparam name="TOptions">The type of <see cref="HttpClientOptions"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="key">
        /// The configuration section key name to use.
        /// If not provided, it will be the <typeparamref name="T"/> type name without the -Options prefix.
        /// (see <see cref="ConfigurationExtensions.DefaultOptionsName(Type)"/>.
        /// </param>
        /// <param name="refitSettings">
        /// Custom <see cref="RefitSettings"/> to apply, if any.</param>
        /// <returns>The configured <see cref="IHttpClientBuilder"/>, that can be further customized.</returns>
        public static IHttpClientBuilder AddRefitClient<TClient, TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? key = null,
            RefitSettings? refitSettings = null)
            where TClient : class
            where TOptions : HttpClientOptions, new()
        {
            services.BindOptionsToConfigurationAndValidate<TOptions>(configuration, key: key);

            var options = configuration.ReadOptionsAndValidate<TOptions>(key);

            if (refitSettings is null)
            {
                if ("xml".Equals(options.Serializer, StringComparison.OrdinalIgnoreCase))
                {
                    refitSettings = new RefitSettings(new XmlContentSerializer());
                }
                else
                {
                    var jsonSerializerOptions = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true,
                    };

                    jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    jsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
                    jsonSerializerOptions.Converters.Add(new JsonNullableTimeSpanConverter());
                    refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(jsonSerializerOptions));
                }
            }

            return services
                .AddSingleton(provider => RequestBuilder.ForType<TClient>(refitSettings))
                .AddHttpClient(typeof(TClient).Name)
                .ConfigureWithOptions<TOptions>(configuration, key: key)
                .AddTypedClient((client, serviceProvider) => RestService.For(client, serviceProvider.GetRequiredService<IRequestBuilder<TClient>>()));
        }
    }
}
