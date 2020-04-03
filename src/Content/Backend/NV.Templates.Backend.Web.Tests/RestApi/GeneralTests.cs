using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NV.Templates.Backend.Web.RestApi.General;
using Refit;
using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests.RestApi
{
    [Collection(TestWebApplicationFactoryCollection.CollectionName)]
    public class GeneralTests
    {
        private readonly TestWebApplicationFactory _factory;

        public GeneralTests(TestWebApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            _factory.OutputHelper = outputHelper;
        }

        internal interface IGeneralApi
        {
            [Get("/api/info")]
            Task<ApplicationInfoModel> GetInfo();

            [Get("/api/health")]
            Task<HttpResponseMessage> GetHealth();

            [Get("/swagger/v1/swagger.json")]
            Task<HttpResponseMessage> GetSwaggerDocument();

            [Get("/attributions.txt")]
            Task<HttpResponseMessage> GetAttributions();
        }

        [Fact]
        public async Task ItShouldGetInfo()
        {
            var api = _factory.GetApiClient<IGeneralApi>();

            var result = await api.GetInfo();
            result.Name!.Should().NotBeNullOrEmpty();
            result.Environment!.Should().NotBeNullOrEmpty();
            result.Version!.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public virtual async Task ItShouldReturnHealthChecks()
        {
            var api = _factory.GetApiClient<IGeneralApi>();

            var response = await api.GetHealth();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public virtual async Task ItShouldReturnSwagger()
        {
            var api = _factory.GetApiClient<IGeneralApi>();

            var response = await api.GetSwaggerDocument();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public virtual async Task ItShouldReturnAttributions()
        {
            var api = _factory.GetApiClient<IGeneralApi>();

            var response = await api.GetAttributions();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
