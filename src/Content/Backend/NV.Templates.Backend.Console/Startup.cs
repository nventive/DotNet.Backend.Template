using Microsoft.Extensions.DependencyInjection;

namespace NV.Templates.Backend.Console
{
    /// <summary>
    /// Startup class that mimics the ASP.NET Core startup pattern.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure Dependency Injection services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }
    }
}
