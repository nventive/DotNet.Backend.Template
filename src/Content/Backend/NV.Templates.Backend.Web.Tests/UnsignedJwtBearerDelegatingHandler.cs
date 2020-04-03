using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace NV.Templates.Backend.Web.Tests
{
    /// <summary>
    /// <see cref="DelegatingHandler"/> that creates unsigned JWT bearer tokens using
    /// a provided <see cref="ClaimsIdentity"/>.
    /// </summary>
    internal class UnsignedJwtBearerDelegatingHandler : DelegatingHandler
    {
        private readonly ClaimsIdentity _identity;

        public UnsignedJwtBearerDelegatingHandler(ClaimsIdentity identity)
        {
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = new JwtSecurityTokenHandler().CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = _identity,
            });
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
