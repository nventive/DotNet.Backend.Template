using System;
using FluentValidation;

namespace NV.Templates.Backend.Core.Framework.Validation
{
    /// <summary>
    /// <see cref="IValidatorFactory"/> implementation that uses <see cref="IServiceProvider"/>.
    /// This is copied here because the original implementation is on FluentValidation.AspNetCore. Grr.
    /// </summary>
    public class ServiceProviderValidatorFactory : ValidatorFactoryBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
