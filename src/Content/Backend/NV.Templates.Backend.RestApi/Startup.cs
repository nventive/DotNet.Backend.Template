using System;
using System.Linq;
using AspNetCoreRequestTracing;
using FluentValidation;
using FluentValidation.AspNetCore;
using HelpDeskId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NV.Templates.Backend.RestApi.Framework.Middlewares;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace NV.Templates.Backend.RestApi
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
            services.AddCore();

            services
                .AddValidatorsFromAssemblyContaining<Startup>()
                .AddSingleton<IHelpDeskIdGenerator, HelpDeskIdGenerator>()
                .Configure<RequestTracingMiddlewareOptions>(_configuration.GetSection(nameof(RequestTracingMiddlewareOptions)));

            services
                .AddResponseCaching()
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddMvcCore(options =>
                {
                    options.Filters.Add(new ResponseCacheAttribute { Location = ResponseCacheLocation.None, NoStore = true });
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddCors()
                .AddJsonFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    };
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                })
                .AddFluentValidation();

            services.AddOpenApi();
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// This method gets called by the runtime.
        /// </summary>
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
                "/health",
                new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });

            app.UseAttributions();

            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3(configure =>
            {
                configure.DocExpansion = "list";
            });
        }
    }
}
