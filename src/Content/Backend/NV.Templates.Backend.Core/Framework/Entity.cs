namespace NV.Templates.Backend.Core.Framework
{
    /// <summary>
    /// Base class for root entities with an Id property.
    /// </summary>
    public abstract class Entity : IIdentifiable
    {
        /// <inheritdoc />
        public string Id { get; set; } = IdGenerator.Generate();

        /// <inheritdoc />
        public override string ToString() => $"[{GetType().Name}] ({Id})";
    }
}
