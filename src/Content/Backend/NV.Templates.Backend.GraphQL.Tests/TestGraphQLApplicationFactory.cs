using System;
using GraphQL.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.GraphQL.Tests
{
    /// <summary>
    /// Custom <see cref="WebApplicationFactory{TEntryPoint}"/> for integration tests.
    /// </summary>
    public class TestGraphQLApplicationFactory : WebApplicationFactory<Startup>
    {
        public ITestOutputHelper TestOutputHelper { get; set; }

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
            builder.ConfigureLogging(logging =>
            {
                logging.AddXUnit(TestOutputHelper);
            });
        }
    }
}
