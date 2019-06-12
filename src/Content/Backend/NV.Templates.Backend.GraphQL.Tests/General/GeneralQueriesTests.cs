using System.Threading.Tasks;
using FluentAssertions;
using HttpClientUtilities;
using NV.Templates.Backend.Core.General;
using NV.Templates.Backend.GraphQL.Framework.Middlewares;
using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.GraphQL.Tests.General
{
    [Collection(TestGraphQLApplicationFactoryCollection.CollectionName)]
    public class GeneralQueriesTests
    {
        private readonly TestGraphQLApplicationFactory _factory;

        public GeneralQueriesTests(TestGraphQLApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _factory.TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldReturnInfo()
        {
            var client = _factory.CreateGraphQLClient();
            var response = await client.PostQueryAsync(@"
                {
                    info {
                        name
                        environment
                        version
                    }
                }
            ");

            response.Errors.Should().BeNullOrEmpty();
            string appInfoName = response.Data.info.name;
            appInfoName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ItShouldReturnSchema()
        {
            var client = _factory.CreateGraphQLClient();
            var response = await client.PostQueryAsync(@"
                {
                    __schema {
                        queryType {
                            name
                        }
                    }
                }
            ");

            response.Errors.Should().BeNullOrEmpty();
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
    }
}
