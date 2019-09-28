using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests.GraphQLApi.General
{
    [Collection(TestWebApplicationFactoryCollection.CollectionName)]
    public class GeneralQueriesTests
    {
        private readonly TestWebApplicationFactory _factory;

        public GeneralQueriesTests(TestWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _factory.OutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldReturnInfo()
        {
            _factory.Server.AllowSynchronousIO = true;
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
    }
}
