using System;
using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Base class for tests that uses the <see cref="CoreServicesFixture"/>.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// public class SampleTests : CoreServiceTests
    /// {
    ///     public SampleTests(CoreServicesFixture fixture, ITestOutputHelper outputHelper)
    ///         : base(fixture, outputHelper)
    ///     {
    ///     }
    ///
    ///     [Fact]
    ///     public async Task TestCase()
    ///     {
    ///         using var scope = Fixture.GetServiceScope(); // This allows resolving of services
    ///         using var context = new HttpRecorderContext(); // Optional - use it to record HttpDependencies
    ///         var myService = scope.ServiceProvider.GetRequiredService<MyService>(); // Resolve service under test
    ///         // ... Perform test and assert.
    ///     }
    /// }
    /// ]]>
    /// </example>
    [Collection(CoreServicesFixtureCollection.CollectionName)]
    public abstract class CoreServiceTests
    {
        protected CoreServiceTests(CoreServicesFixture fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            Fixture.OutputHelper = outputHelper;
        }

        /// <summary>
        /// Gets the <see cref="CoreServicesFixture"/>.
        /// </summary>
        protected CoreServicesFixture Fixture { get; }
    }
}
