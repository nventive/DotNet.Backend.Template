using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Web.Framework.GraphQL;
using NV.Templates.Backend.Web.GraphQLApi;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class GraphQLServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GraphQL services.
        /// </summary>
        internal static IServiceCollection AddGraphQLApi(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IDependencyResolver, HttpContextAccessorDependencyResolver>()
                .AddSingleton<GraphQLSchema>()
                .AddGraphQL(options =>
                {
                    configuration.GetSection(nameof(GraphQLOptions))?.Bind(options);
                })
                .AddGraphTypes()
                .AddUserContextBuilder<GraphQLUserContextBuilder>()
                .AddDataLoader()
                .Services
                .AddTransient(typeof(IGraphQLExecuter<>), typeof(GraphQLExecuter<>));

            return services;
        }
    }
}
