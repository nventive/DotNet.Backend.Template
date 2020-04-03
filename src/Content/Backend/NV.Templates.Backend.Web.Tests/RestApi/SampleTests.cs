using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using NV.Templates.Backend.Web.Framework.Models;
using Refit;
using Xunit;
using Xunit.Abstractions;
using static NV.Templates.Backend.Web.RestApi.SampleController;

namespace NV.Templates.Backend.Web.Tests.RestApi
{
    [Collection(TestWebApplicationFactoryCollection.CollectionName)]
    public class SampleTests
    {
        private readonly TestWebApplicationFactory _factory;

        public SampleTests(TestWebApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            _factory.OutputHelper = outputHelper;
        }

        internal interface ISampleApi
        {
            [Get("/api/v1/samples")]
            Task<ContinuationEnumerableModel<SampleModel>> FindSamples();

            [Get("/api/v1/samples/{id}")]
            Task<SampleModel> GetSample(string id);
        }

        [Fact]
        public async Task ItShouldFindSample()
        {
            var api = _factory.GetApiClient<ISampleApi>(new ClaimsIdentity());

            var result = await api.FindSamples();
            result.Items.Should().BeEmpty();
            result.ContinuationToken.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ItShouldGetSample()
        {
            var api = _factory.GetApiClient<ISampleApi>(new ClaimsIdentity());

            var result = await api.GetSample("my-id");
            result.Id.Should().Be("my-id");
        }
    }
}
