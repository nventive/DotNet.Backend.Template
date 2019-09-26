using System;

namespace NV.Templates.Backend.Core.Framework.Services
{
    /// <summary>
    /// Mark a service for registration in the DI with a Transient lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegisterTransientServiceAttribute : Attribute
    {
    }
}
