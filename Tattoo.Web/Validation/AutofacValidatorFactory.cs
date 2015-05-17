#region Using Directives

using System;
using Autofac;
using FluentValidation;

#endregion

namespace Tattoo.Web.Validation
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer _container;

        public AutofacValidatorFactory(IContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            using (var scope = _container.BeginLifetimeScope("AutofacWebRequest"))
            {
                return scope.IsRegistered(validatorType)
                    ? scope.Resolve(validatorType) as IValidator
                    : null;
            }
        }
    }
}