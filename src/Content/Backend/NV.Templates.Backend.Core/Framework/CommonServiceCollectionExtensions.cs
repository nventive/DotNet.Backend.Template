using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommonServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Redis cache services if set up, otherwise fallback on distributed memry cache.
        /// </summary>
        public static IServiceCollection AddRedisCacheIfPresent(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheConnectionString = configuration.GetConnectionString("Cache");
            if (string.IsNullOrWhiteSpace(cacheConnectionString))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheConnectionString;
                });
            }

            return services;
        }
    }
}
