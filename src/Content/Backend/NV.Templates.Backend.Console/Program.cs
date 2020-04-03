using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NV.Templates.Backend.Console
{
    [Command]
    [Subcommand(
        typeof(General.InfoCommand),
        typeof(General.AttributionsCommand))]
    public class Program
    {
        public static Task<int> Main(string[] args) => new HostBuilder()
            .ConfigureAppConfiguration((hostBuilderContext, config) =>
            {
                var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
                config
                    .AddLocalSettings(rootProjectPath)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Program>();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }

                hostBuilderContext.HostingEnvironment.ApplicationName = typeof(Program).Assembly.GetName().Name;
            })
            .ConfigureLogging((_, builder) =>
            {
                builder.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddOptions();
                new Startup(context.Configuration).ConfigureServices(services);
            })
            .RunCommandLineApplicationAsync<Program>(args);

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify a subcommand.");
            app.ShowHelp();
            return 1;
        }
    }
}
