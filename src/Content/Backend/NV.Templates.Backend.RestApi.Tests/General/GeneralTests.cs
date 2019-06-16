using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HttpClientUtilities;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NV.Templates.Backend.RestApi.Framework.Middlewares;
using NV.Templates.Backend.RestApi.General;
using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.RestApi.Tests.General
{
    [Collection(TestWebApplicationFactoryCollection.CollectionName)]
    public class GeneralTests
    {
        private readonly TestWebApplicationFactory _factory;

        public GeneralTests(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _factory.TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldReturnApplicationInfo()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(FluentUriBuilder.ForPath("/info"));

            response.EnsureSuccessStatusCode();
            var appInfo = await response.Content.ReadAsAsync<ApplicationInfoModel>();
            appInfo.Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ItShouldReturnOperationIdHeader()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(FluentUriBuilder.ForPath("/info"));

            response.Headers.GetValues(OperationContextMiddleware.OperationIdHeader).FirstOrDefault().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ItShouldReturnAttributions()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(FluentUriBuilder.ForPath(AttributionsHandler.Path));

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
            var attribution = await response.Content.ReadAsStringAsync();
            attribution.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ItShouldPerformHealthChecks()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(FluentUriBuilder.ForPath("/health"));

            response.EnsureSuccessStatusCode();
            var healthReport = await response.Content.ReadAsAsync<HealthReport>();
            healthReport.Status.Should().Be(HealthStatus.Healthy);
        }

        [Fact]
        public async Task ItShouldReturnSwaggerDoc()
        {
            var client = _factory.CreateClient();

            var apiVersionDescriptionProvider = _factory.Server.Host.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(x => x.GroupName))
            {
                var uri = FluentUriBuilder
                    .ForPath("/swagger")
                    .WithSegment(description.GroupName)
                    .WithSegment("swagger.json");
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
