using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NV.Templates.Backend.Web.Framework.Security
{
    /// <summary>
    /// Options for Authentication configuration.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the required JWT token Audience to check.
        /// </summary>
        [Required]
        public string? JwtAudience { get; set; }

        /// <summary>
        /// Gets or sets the required JWT token Authority to check.
        /// </summary>
        [Required]
        public string? JwtAuthority { get; set; }

        /// <summary>
        /// Gets or sets the OAuth2 authorization url to use.
        /// This is used for OpenApi OAuth 2 support.
        /// </summary>
        public Uri? AuthorizationUrl { get; set; }

        /// <summary>
        /// Gets or sets the Scopes to use for User authentication.
        /// This is used for OpenApi OAuth 2 support.
        /// </summary>
        public IEnumerable<string>? UserAuthenticationScopes { get; set; }

        /// <summary>
        /// Gets or sets the client id of the client application.
        /// This is used for OpenApi OAuth 2 support.
        /// </summary>
        public string? ClientClientId { get; set; }
    }
}
