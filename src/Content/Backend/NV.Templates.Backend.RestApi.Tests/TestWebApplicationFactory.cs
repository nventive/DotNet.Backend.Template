using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.RestApi.Tests
{
    /// <summary>
    /// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> for integration tests.
    /// </summary>
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public MediaTypeFormatter MediaTypeFormatter { get; } = CreateMediaTypeFormatter();

        public ITestOutputHelper TestOutputHelper { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.AddXUnit(TestOutputHelper);
            });
        }

        private static MediaTypeFormatter CreateMediaTypeFormatter()
        {
            var formatter = new JsonMediaTypeFormatter();
            formatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/problem+json"));

            return formatter;
        }
    }
}
