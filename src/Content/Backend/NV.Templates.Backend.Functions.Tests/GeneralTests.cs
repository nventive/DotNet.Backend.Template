using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Xunit;

namespace NV.Templates.Backend.Functions.Tests
{
    public class GeneralTests
    {
        [Fact]
        public async Task ItShouldGetInfo()
        {
            var function = new GeneralFunctions(Mock.Of<IApplicationInfo>());

            var result = await function.GetInfo(Mock.Of<HttpRequestData>());

            result.Should().BeOfType<JsonResult>();
        }
    }
}
