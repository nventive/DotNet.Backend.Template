using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using GraphQL.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Web.Tests
{
    /// <summary>
    /// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> for integration tests.
    /// </summary>
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>, ITestOutputHelperAccessor
    {
        public MediaTypeFormatter MediaTypeFormatter { get; } = CreateMediaTypeFormatter();

        public ITestOutputHelper? OutputHelper { get; set; }

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
