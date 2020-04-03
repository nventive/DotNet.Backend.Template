using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Moq;
using NV.Templates.Backend.Core.General;
using Xunit;

namespace NV.Templates.Backend.Functions.Tests
{
    public class GeneralTests
    {
        [Fact]
        public async Task ItShouldGetInfo()
        {
            var function = new GeneralFunctions(
                Mock.Of<IApplicationInfo>(),
                Mock.Of<IOperationContext>());

            var result = await function.GetInfo(Mock.Of<HttpRequest>(), new ExecutionContext());

            result.Should().BeOfType<JsonResult>();
        }
    }
}
