using System;
#if Auth
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
#endif
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
#if Auth
using System.Security.Claims;
#endif
#if GraphQLApi
using GraphQL.Client;
#endif
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#if Auth
using Microsoft.IdentityModel.Tokens;
#endif
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests
{
    /// <summary>
    /// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> for integration tests.
    /// </summary>
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>, ITestOutputHelperAccessor
    {
        public MediaTypeFormatter MediaTypeFormatter { get; } = CreateMediaTypeFormatter();

        public ITestOutputHelper OutputHelper { get; set; }

#if Auth
        /// <summary>
        /// Creates a new instance of an <see cref="HttpClient"/> that can be used to send <see cref="HttpRequestMessage"/>
        /// to the server with valid authentication information using <paramref name="identity"/>.
        /// </summary>
        /// <param name="identity">The complete set of claims to use.</param>
        public HttpClient CreateClient(ClaimsIdentity identity)
            => CreateDefaultClient(new UnsignedJwtBearerDelegatingHandler(identity));
#endif

#if GraphQLApi
        /// <summary>
        /// Creates a connected <see cref="GraphQLClient"/>.
        /// </summary>
        public GraphQLClient CreateGraphQLClient()
        {
            // This takes care of Server creation.
            _ = CreateClient();
            return new GraphQLClient(
                new GraphQLClientOptions
                {
                    EndPoint = new Uri(Server.BaseAddress, "/graphql"),
                    HttpMessageHandler = Server.CreateHandler(),
                });
        }
#endif

#if GraphQLApi && Auth
        /// <summary>
        /// Creates a connected <see cref="GraphQLClient"/> with valid authentication information using <paramref name="identity"/>.
        /// </summary>
        /// <param name="identity">The complete set of claims to use.</param>
        public GraphQLClient CreateGraphQLClient(ClaimsIdentity identity)
        {
            // This takes care of Server creation.
            _ = CreateClient();
            return new GraphQLClient(
                new GraphQLClientOptions
                {
                    EndPoint = new Uri(Server.BaseAddress, "/graphql"),
                    HttpMessageHandler = new UnsignedJwtBearerDelegatingHandler(identity)
                    {
                        InnerHandler = Server.CreateHandler(),
                    },
                });
        }
#endif

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureLogging(logging =>
                {
                    logging.AddXUnit(OutputHelper);
                })
                .ConfigureTestServices(services =>
                {
                    // Adds test-specific services configuration.
#if Auth

                    // We replace the authentication scheme with a JwtBearer that does not validate signatures.
                    // See UnsignedJwtBearerDelegatingHandler for token generation.
                    services
                        .AddAuthentication(options =>
                        {
                            options.DefaultScheme = "NonValidatedBearer";
                            options.DefaultChallengeScheme = "NonValidatedBearer";
                        })
                        .AddJwtBearer("NonValidatedBearer", options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                // This effectively bypass token signature validation.
                                SignatureValidator = (token, _) => new JwtSecurityTokenHandler().ReadJwtToken(token),
                                ValidateAudience = false,
                                ValidateIssuer = false,
                            };
                        });
#endif
                });
        }

        private static MediaTypeFormatter CreateMediaTypeFormatter()
        {
            var formatter = new JsonMediaTypeFormatter();
            formatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/problem+json"));

            return formatter;
        }
    }
}
