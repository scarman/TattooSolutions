#region Using Directives

using FluentValidation;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Validation.AccountValidators
{
    public class PasswordRecoveryValidator : AbstractValidator<PasswordRecoveryModel>
    {
        public PasswordRecoveryValidator()
        {
            RuleFor(item => item.Email).NotEmpty().EmailAddress();
        }
    }
}