using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NV.Templates.Backend.Console.General;
using NV.Templates.Backend.Core.General;
using Xunit;

namespace NV.Templates.Backend.Console.Tests.General
{
    public class InfoCommandTests
    {
        [Fact]
        public async Task ItShouldLogApplicationInfo()
        {
            var applicationInfoMock = new Mock<IApplicationInfo>();
            var logger = new Mock<ILogger<InfoCommand>>();
            var command = new InfoCommand(applicationInfoMock.Object, logger.Object);

            await command.OnExecuteAsync();
        }
    }
}
