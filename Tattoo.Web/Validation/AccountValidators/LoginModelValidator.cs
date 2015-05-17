#region Using Directives

using FluentValidation;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Validation.AccountValidators
{
    public class LoginModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginModelValidator()
        {
            RuleFor(model => model.Email)
                .EmailAddress()
                .NotNull();
            RuleFor(model => model.Password)
                .NotNull();
        }
    }
}