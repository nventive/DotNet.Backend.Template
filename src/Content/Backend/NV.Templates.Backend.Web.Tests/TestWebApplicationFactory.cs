using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using HttpTracing;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests
{
    /// <summary>
    /// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> for integration tests.
    /// </summary>
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>, ITestOutputHelperAccessor
    {
        public ITestOutputHelper? OutputHelper { get; set; }

        /// <summary>
        /// Create a Refit client based on <typeparamref name="T"/>,
        /// connected to the API under test.
        /// </summary>
        /// <typeparam name="T">The Refit interface to use.</typeparam>
        /// <param name="claimsIdentity">If passed, will also include a JWT Bearer token with that identity.</param>
        public T GetApiClient<T>(ClaimsIdentity? claimsIdentity = null)
        {
            var delegatingHandlers = new List<DelegatingHandler>();
            if (claimsIdentity != null)
            {
                delegatingHandlers.Add(new UnsignedJwtBearerDelegatingHandler(claimsIdentity));
            }

            delegatingHandlers.Add(new HttpTracingDelegatingHandler(Services.GetRequiredService<ILogger<T>>(), bufferRequests: true));
            return RestService.For<T>(
                CreateDefaultClient(delegatingHandlers.ToArray()),
                new RefitSettings
                {
                    ContentSerializer = new SystemTextJsonContentSerializer(
                        Services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions),
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureAppConfiguration(config =>
                {
                    var rootProjectPath = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));
                    config.AddJsonFile(
                        Path.Combine(rootProjectPath, "LocalSettings.Test.json"),
                        optional: true,
                        reloadOnChange: true);
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddXUnit(OutputHelper!);
                    logging.AddFilter((category, level) => category!.EndsWith("TraceHandler", StringComparison.OrdinalIgnoreCase));
                })
                .ConfigureTestServices(services =>
                {
#if Auth
                    // We replace the authentication scheme with a JwtBearer that does not validate signatures.
                    // See UnsignedJwtBearerDelegatingHandler for token generation.
                    services
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = "NonValidatedBearer";
                            options.DefaultChallengeScheme = "NonValidatedBearer";
                        })
                        .AddJwtBearer("NonValidatedBearer", options =>
                        {
                            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                // This effectively bypass token signature validation.
                                SignatureValidator = (token, _) => new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(token),
                                ValidateAudience = false,
                                ValidateIssuer = false,
                            };
                        });
#endif
                    services.AddHttpRecorderContextSupport();
                    // Adds test-specific services configuration.
                });
        }
    }
}
