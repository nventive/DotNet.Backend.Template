using System;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace NV.Templates.Backend.Console.General
{
    [Command(Name = "attributions", Description = "Outputs 3rd party licenses information", ThrowOnUnexpectedArgument = false)]
    public class AttributionsCommand
    {
        private readonly ILogger<AttributionsCommand> _logger;

        public AttributionsCommand(ILogger<AttributionsCommand> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> OnExecuteAsync()
        {
            var fileProvider = new EmbeddedFileProvider(typeof(AttributionsCommand).Assembly);
            var attributions = fileProvider.GetFileInfo("Properties.ATTRIBUTIONS.txt");
            using (var streamReader = new StreamReader(attributions.CreateReadStream()))
            {
                _logger.LogInformation(await streamReader.ReadToEndAsync());
            }

            return 0;
        }
    }
}
