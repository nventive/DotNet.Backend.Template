using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using NV.Templates.Backend.Web.Framework.GraphQL;
using NV.Templates.Backend.Web.GraphQLApi;
#if Auth
using NV.Templates.Backend.Web.GraphQLApi.Security;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GraphQLServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GraphQL services.
        /// </summary>
        public static IServiceCollection AddGraphQLApi(this IServiceCollection services, IConfiguration configuration)
        {
            // This is a fix until this is resolve in GraphQL.
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

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

#if Auth
            services
                .AddSingleton<GraphQL.Authorization.IAuthorizationEvaluator, GraphQL.Authorization.AuthorizationEvaluator>()
                .AddTransient<GraphQL.Validation.IValidationRule, GraphQL.Authorization.AuthorizationValidationRule>()
                .AddSingleton(x =>
                {
                    var authorizationSettings = new GraphQL.Authorization.AuthorizationSettings();
                    authorizationSettings.AddPolicy(AuthorizationPolicyNames.RequireUser, y => y.AddRequirement(new RequireUserAuthorizationRequirement()));
                    return authorizationSettings;
                });
#endif

            return services;
        }
    }
}
