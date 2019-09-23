using System;
using AspNetCoreRequestTracing;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NV.Templates.Backend.Web.Framework.Middlewares;
using NV.Templates.Backend.Web.GraphQLApi;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace NV.Templates.Backend.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();

            services.AddWeb();

            services.AddRestApi();

            services.AddGraphQLApi(_configuration);

            services.AddOpenApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHsts()
               .UseHttpsRedirection()
               .UseCors();

            app.UseRequestTracing();
            app.UseExceptionHandler(ExceptionHandler.ConfigureExceptionHandling);

            app.UseMiddleware<OperationContextMiddleware>();

            app.UseResponseCaching();

            app.UseHealthChecks(
                "/api/health",
                new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });

            app.UseAttributions();

            app
                .UseGraphQL<GraphQLSchema>()
                .UseGraphiQLServer(new GraphiQLOptions())
                .UseGraphQLVoyager(new GraphQLVoyagerOptions { Path = "/graphql-voyager" });

            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3(configure =>
            {
                configure.DocExpansion = "list";
            });
        }
    }
}
