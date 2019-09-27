namespace System.Security.Principal
{
    /// <summary>
    /// <see cref="IIdentity"/> extension methods.
    /// </summary>
    public static class IdentityExtensions
    {
        public static string GetUserName(this IIdentity? identity) => identity != null && identity.IsAuthenticated ? identity.Name : "Anonymous";
    }
}
