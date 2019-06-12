using System;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NV.Templates.Backend.GraphQL.Framework.GraphQL
{
    /// <summary>
    /// <see cref="IDependencyResolver"/> that uses the current <see cref="HttpContext.RequestServices"/>
    /// to resolve dependencies in GraphQL types.
    /// We do this to allow resolving transient and scoped services in the GraphQL types which
    /// should be singletons.
    /// </summary>
    internal class HttpContextAccessorDependencyResolver : IDependencyResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextAccessorDependencyResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public T Resolve<T>() => _httpContextAccessor.HttpContext.RequestServices.GetService<T>();

        public object Resolve(Type type) => _httpContextAccessor.HttpContext.RequestServices.GetService(type);
    }
}
