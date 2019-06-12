using FluentValidation;

namespace NV.Templates.Backend.Core.Framework.Validation
{
    /// <summary>
    /// Base class for <see cref="IValidator{T}"/> that validates <see cref="Entity"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Entity"/> type.</typeparam>
    public abstract class EntityValidator<T> : AbstractValidator<T>
        where T : Entity
    {
        protected EntityValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
