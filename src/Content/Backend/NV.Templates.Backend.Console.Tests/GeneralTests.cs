using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NV.Templates.Backend.Console.General;
using NV.Templates.Backend.Core.General;
using Xunit;

namespace NV.Templates.Backend.Console.Tests
{
    public class GeneralTests
    {
        [Fact]
        public async Task ItShouldRunInfoCommand()
        {
            var command = new InfoCommand(
                Mock.Of<IApplicationInfo>(),
                Mock.Of<ILogger<InfoCommand>>());

            Func<Task<int>> act = async () => await command.OnExecuteAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ItShouldRunAttributionsCommand()
        {
            var command = new AttributionsCommand(
                Mock.Of<ILogger<AttributionsCommand>>());

            Func<Task<int>> act = async () => await command.OnExecuteAsync();
            await act.Should().NotThrowAsync();
        }
    }
}
