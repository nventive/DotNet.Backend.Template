using System.Security.Principal;

namespace NV.Templates.Backend.Core.Framework.Auth
{
    /// <summary>
    /// <see cref="IIdentity"/> for the System user.
    /// </summary>
    public sealed class SystemIdentity : IIdentity
    {
        private SystemIdentity()
        {
        }

        public static SystemIdentity Instance { get; } = new SystemIdentity();

        public string AuthenticationType => "System";

        public bool IsAuthenticated => true;

        public string Name => "System";
    }
}
