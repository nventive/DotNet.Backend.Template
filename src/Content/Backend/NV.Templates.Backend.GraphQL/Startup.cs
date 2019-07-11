using System;
using FluentValidation;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Voyager;
using HelpDeskId;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NV.Templates.Backend.GraphQL.Framework.GraphQL;
using NV.Templates.Backend.GraphQL.Framework.Middlewares;
using NV.Templates.Backend.GraphQL.Framework.Telemetry;

namespace NV.Templates.Backend.GraphQL
{
    /// <summary>
    /// ASP.NET Core startup.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configure Dependency Injection services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Core assembly services
            services.AddCore();

            // Common services
            services
                .AddValidatorsFromAssemblyContaining<Startup>()
                .AddSingleton<IHelpDeskIdGenerator, HelpDeskIdGenerator>();

            // ASP.NET Core services
            services
                .AddHttpContextAccessor()
                .AddCors()
                .AddRouting();

            // Telemetry
            services.AddSingleton<ITelemetryInitializer, HttpContextTelemetryInitializer>();

            // GraphQL services
            services
                .AddSingleton<IDependencyResolver, HttpContextAccessorDependencyResolver>()
                .AddSingleton<GraphQLSchema>()
                .AddGraphQL(options =>
                {
                    _configuration.GetSection(nameof(GraphQLOptions))?.Bind(options);
                })
                .AddGraphTypes()
                .AddUserContextBuilder<GraphQLUserContextBuilder>()
                .AddDataLoader()
                .Services
                .AddTransient(typeof(IGraphQLExecuter<>), typeof(GraphQLExecuter<>));
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// This method gets called by the runtime.
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AbortGraphQLWebSocketMiddleware>();

            app.UseHsts()
               .UseHttpsRedirection()
               .UseCors();

            app.UseMiddleware<OperationContextMiddleware>();

            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });

            app.UseAttributions();

            app
                .UseGraphQL<GraphQLSchema>()
                .UseGraphiQLServer(new GraphiQLOptions())
                .UseGraphQLVoyager(new GraphQLVoyagerOptions { Path = "/voyager" });

            // Small helper routes
            app.UseRouter(router =>
            {
                router.MapGet(PathString.Empty, async (request, response, routeData) =>
                {
                    response.StatusCode = StatusCodes.Status307TemporaryRedirect;
                    response.Headers["Location"] = $"{request.Scheme}://{request.Host}/graphiql";
                });
            });
        }
    }
}
