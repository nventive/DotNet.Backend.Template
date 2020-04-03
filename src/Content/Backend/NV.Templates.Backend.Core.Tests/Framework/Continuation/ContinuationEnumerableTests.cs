using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using NV.Templates.Backend.Core.Framework.Continuation;
using Xunit;

namespace NV.Templates.Backend.Core.Tests.Framework.Continuation
{
    public class ContinuationEnumerableTests
    {
        [Fact]
        public void ItShouldEncapsulateEnumerable()
        {
            var source = new List<string> { new Faker().Lorem.Word() };
            var continuationToken = new Faker().Random.Hash();

            var result = new ContinuationEnumerable<string>(source, continuationToken);

            result.ContinuationToken.Should().Be(continuationToken);
            result.As<IEnumerable<string>>().Should().ContainInOrder(source);
        }
    }
}
