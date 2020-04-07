using Xunit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests
{
    /// <summary>
    /// Base class for tests that uses the <see cref="TestWebApplicationFactory"/>.
    /// </summary>
    [Collection(TestWebApplicationFactoryCollection.CollectionName)]
    public abstract class WebTests
    {
        protected WebTests(TestWebApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            Factory = factory;
            Factory.OutputHelper = outputHelper;
        }

        /// <summary>
        /// Gets the <see cref="TestWebApplicationFactory"/>.
        /// </summary>
        protected TestWebApplicationFactory Factory { get; }
    }
}
