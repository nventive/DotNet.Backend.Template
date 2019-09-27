namespace NV.Templates.Backend.Core.Framework
{
    /// <summary>
    /// Base class for root entities with Id and Name properties.
    /// </summary>
    public abstract class NamedEntity : Entity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <inheritdoc />
        public override string ToString() => $"{base.ToString()} name: {Name}";
    }
}
