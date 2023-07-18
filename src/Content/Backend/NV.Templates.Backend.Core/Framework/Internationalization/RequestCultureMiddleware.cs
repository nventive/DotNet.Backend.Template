using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace NV.Templates.Backend.Core.Framework.Internationalization
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
            var culture = requestCultureFeature!.RequestCulture.Culture;

            CultureConfig.SetCulture(culture);

            context.Request.Headers["Accept-Language"] = culture.Name;

            await _next.Invoke(context);
        }
    }
}
