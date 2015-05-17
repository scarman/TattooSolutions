#region Using Directives

using Autofac;
using FluentValidation;
using Tattoo.Web.Validation.AccountValidators;

#endregion

namespace Tattoo.Web.Validation
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var findValidatorsInAssembly =
                AssemblyScanner.FindValidatorsInAssembly(typeof (RegisterModelValidator).Assembly);
            foreach (var item in findValidatorsInAssembly)
            {
                builder.RegisterType(item.ValidatorType).As(item.InterfaceType);
            }
        }
    }
}