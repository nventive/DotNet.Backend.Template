﻿using System;
using AspNetCoreRequestTracing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(RestApiConventions))]

namespace NV.Templates.Backend.Web
{
    public class Startup
    {
#if SPA
        private const string ClientAppLocation = "ClientApp";
#endif
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRedisCacheIfPresent(_configuration)
                .AutoRegisterServicesFromAssembly()
                .AddCore(_configuration)
                .AddWeb(_configuration)
                .AddInternationalization(_configuration)
                .AddRestApi()
                .AddOpenApi(_configuration);
#if Auth
            services
                .BindOptionsToConfigurationAndValidate<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>(_configuration)
                .BindOptionsToConfigurationAndValidate<NSwag.AspNetCore.OAuth2ClientSettings>(_configuration);

            services
                .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
#endif
#if SPA
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = $"{ClientAppLocation}/dist";
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHsts()
               .UseHttpsRedirection()
               .UseRouting()
               .UseCors(opt =>
               {
                   opt.AllowAnyOrigin();
               });

            app.UseInternationalization();

            app.UseRequestTracing()
               .UseExceptionHandler(ExceptionHandler.ConfigureExceptionHandling)
               .UseResponseCaching();
#if Auth
            app.UseAuthentication()
               .UseAuthorization();
#endif
            app.UseMiddleware<OperationContextMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(
                    HealthChecksResponseWriter.HealthChecksEndpoint,
                    new HealthCheckOptions { ResponseWriter = HealthChecksResponseWriter.WriteResponse });
                endpoints.MapAttributions();
#if !SPA
                endpoints.MapRootToOpenApiUi();
#endif
            });

            app.UseCommonOpenApi();
#if SPA
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = ClientAppLocation;

                if (env.IsDevelopment())
                {
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                    // spa.UseAngularCliServer(npmScript: "start");
                }
            });
#endif
        }
    }
}
