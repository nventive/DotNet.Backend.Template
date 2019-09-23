namespace NV.Templates.Backend.Core.Framework
{
    /// <summary>
    /// Represents an entity that have an Id of type <see cref="string"/> which is assignable.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        string Id { get; }
    }
}
