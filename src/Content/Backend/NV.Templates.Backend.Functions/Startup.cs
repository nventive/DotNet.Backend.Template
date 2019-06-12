using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NV.Templates.Backend.Functions.Startup))]

namespace NV.Templates.Backend.Functions
{
    /// <summary>
    /// Startup class that mimics the ASP.NET Core startup pattern.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddCore();
        }
    }
}
