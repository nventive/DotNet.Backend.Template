using System.Security.Principal;

namespace NV.Templates.Backend.Core.Framework.Auth
{
    /// <summary>
    /// <see cref="IPrincipal"/> that represents the System user.
    /// </summary>
    public sealed class SystemPrincipal : IPrincipal
    {
        private SystemPrincipal()
        {
        }

        public static SystemPrincipal Instance { get; } = new SystemPrincipal();

        public IIdentity Identity => SystemIdentity.Instance;

        public bool IsInRole(string role) => true;
    }
}
