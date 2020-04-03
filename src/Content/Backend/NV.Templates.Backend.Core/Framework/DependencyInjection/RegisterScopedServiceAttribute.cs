using System;

namespace NV.Templates.Backend.Core.Framework.DependencyInjection
{
    /// <summary>
    /// Mark a service for registration in the DI with a Scoped lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegisterScopedServiceAttribute : Attribute
    {
    }
}
