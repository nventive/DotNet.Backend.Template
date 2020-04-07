using Xunit;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Test collection for tests using <see cref="CoreServicesFixture"/>.
    /// </summary>
    [CollectionDefinition(CollectionName)]
    public class CoreServicesFixtureCollection : ICollectionFixture<CoreServicesFixture>
    {
        public const string CollectionName = nameof(CoreServicesFixtureCollection);
    }
}
