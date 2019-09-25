using System;
using AspNetCoreRequestTracing;
#if GraphQLApi
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Voyager;
#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NV.Templates.Backend.Web.Framework.Middlewares;
#if GraphQLApi
using NV.Templates.Backend.Web.GraphQLApi;
#endif

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
#if RestApi
            services.AddRestApi();
            services.AddOpenApi();
#endif
#if GraphQLApi
            services.AddGraphQLApi(_configuration);
#endif
#if SPA
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
#endif
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

            app.UseHealthChecks(
                "/api/health",
                new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });

            app.UseAttributions();
#if GraphQLApi
            app
                .UseGraphQL<GraphQLSchema>()
                .UseGraphiQLServer(new GraphiQLOptions())
                .UseGraphQLVoyager(new GraphQLVoyagerOptions { Path = "/graphql-voyager" });
#endif
#if RestApi
            app.UseResponseCaching();
            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3(configure =>
            {
                configure.DocExpansion = "list";
            });
#endif
#if SPA
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (app.ApplicationServices.GetRequiredService<IHostingEnvironment>().IsDevelopment())
                {
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                    // spa.UseAngularCliServer(npmScript: "start");
                }
            });
#endif
        }
    }
}
