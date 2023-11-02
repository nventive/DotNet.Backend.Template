using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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
            // Arrange
            var applicationInfoMock = Mock.Of<IApplicationInfo>();
            var httpRequestMock = new Mock<HttpRequest>();
            var functionContextMock = new Mock<FunctionContext>();

            // Create an instance of GeneralFunctions
            var generalFunctions = new GeneralFunctions(applicationInfoMock);

            // Act
            var result = await generalFunctions.GetInfo(httpRequestMock.Object, functionContextMock.Object);

            // Assert
            result.Should().BeAssignableTo<JsonResult>();
        }
    }
}
