using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace NV.Templates.Backend.Web.Framework.Middlewares
{
    internal static class AttributionsHandler
    {
        /// <summary>
        /// The path at which the attributions.txt is exposed.
        /// </summary>
        public const string Path = "/attributions.txt";

        /// <summary>
        /// Configures the pipeline to return the content of the ATTRIBUTIONS.txt file
        /// at the <see cref="Path"/> (/attributions.txt).
        /// </summary>
        public static IEndpointRouteBuilder MapAttributions(this IEndpointRouteBuilder endpoints)
        {
            if (endpoints is null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            endpoints.MapGet(
                Path,
                async context =>
                {
                    var fileProvider = new EmbeddedFileProvider(typeof(AttributionsHandler).Assembly);
                    var attributions = fileProvider.GetFileInfo("Properties.ATTRIBUTIONS.txt");
                    context.Response.ContentType = "text/plain";
                    using var stream = attributions.CreateReadStream();
                    await stream.CopyToAsync(context.Response.Body);
                });
            return endpoints;
        }
    }
}
