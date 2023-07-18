using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NV.Templates.Backend.Core.Configuration;
using NV.Templates.Backend.Core.Framework.Internationalization;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCommonOpenApi(this IApplicationBuilder app)
        {
            var conf = app.ApplicationServices.GetService<IOptions<BackendOptions>>();

            if (conf?.Value.EnableSwagger ?? false)
            {
                app.UseOpenApi();
                app.UseSwaggerUi3(configure =>
                {
                    configure.DocExpansion = "list";
#if Auth
                    configure.OAuth2Client = app.ApplicationServices.GetRequiredService<IOptions<NSwag.AspNetCore.OAuth2ClientSettings>>().Value;
#endif
                });
            }

            return app;
        }

        public static IApplicationBuilder UseInternationalization(this IApplicationBuilder app)
        {
            return app
                .UseStaticFiles()
                .UseRequestLocalization(options =>
                {
                    options.SupportedCultures = CultureConfig.SupportedCultures.ToArray();
                    options.SupportedUICultures = CultureConfig.SupportedCultures.ToArray();
                    options.DefaultRequestCulture = new RequestCulture(CultureConfig.DefaultCulture);
                    options.AddInitialRequestCultureProvider(new RequestSupportedCultureProvider());
                })
                .UseMiddleware<RequestCultureMiddleware>();
        }

        private class RequestSupportedCultureProvider : RequestCultureProvider
        {
            public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
            {
                IEnumerable<(double score, CultureInfo culture)> qvps = Array.Empty<(double, CultureInfo)>();
                if (httpContext.Request.Query.TryGetValue("culture", out var cultureFromQuery))
                {
                    qvps = qvps.Append((1, new CultureInfo(cultureFromQuery!)));
                }

                if (httpContext.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguageHeaderValue))
                {
                    qvps = qvps.Concat(acceptLanguageHeaderValue
                        .ToString()
                        .Split(',')
                        .Select(rawQvp => (parsed: StringWithQualityHeaderValue.TryParse(rawQvp, out var qvp), qvp))
                        .Where(item => item.parsed)
                        .Select(item => (item.qvp!.Quality ?? 1, new CultureInfo(item.qvp.Value))));
                }

                var culture = qvps
                    .OrderByDescending(qvp => qvp.score)
                    .Select(qvp => (matched: CultureConfig.TryMatchCulture(qvp.culture.Name, out var match), match))
                    .FirstOrDefault(mvp => mvp.matched).match ?? CultureConfig.DefaultCulture;

                return new ProviderCultureResult(culture.Name);
            }
        }
    }
}
