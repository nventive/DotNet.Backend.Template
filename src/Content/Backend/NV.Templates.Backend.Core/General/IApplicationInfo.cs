namespace NV.Templates.Backend.Core.General
{
    /// <summary>
    /// Application information.
    /// </summary>
    public interface IApplicationInfo
    {
        /// <summary>
        /// Gets the application name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the application environment.
        /// </summary>
        string Environment { get; }
    }
}
