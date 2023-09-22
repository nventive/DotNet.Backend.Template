using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInternationalization(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddLocalization()
                .AddSingleton<IStringLocalizer>(sp => sp.GetRequiredService<IStringLocalizer<SharedResources>>())
                .AddSingleton<IStringLocalizerEx, StringLocalizerEx>();
        }
    }
}
