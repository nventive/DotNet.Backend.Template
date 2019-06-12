using Xunit;

namespace NV.Templates.Backend.GraphQL.Tests
{
    [CollectionDefinition(CollectionName)]
    public class TestGraphQLApplicationFactoryCollection : ICollectionFixture<TestGraphQLApplicationFactory>
    {
        public const string CollectionName = nameof(TestGraphQLApplicationFactory);
    }
}
