using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace NV.Templates.Backend.Console.General
{
    [Command(
        Name = "info",
        Description = "Get application information",
        UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class InfoCommand
    {
        private readonly IApplicationInfo _applicationInfo;
        private readonly ILogger<InfoCommand> _logger;

        public InfoCommand(
            IApplicationInfo applicationInfo,
            ILogger<InfoCommand> logger)
        {
            _applicationInfo = applicationInfo ?? throw new ArgumentNullException(nameof(applicationInfo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> OnExecuteAsync()
        {
            _logger.LogInformation($"{_applicationInfo.Name} ({_applicationInfo.Version}) in {_applicationInfo.Environment}");

            return 0;
        }
    }
}
