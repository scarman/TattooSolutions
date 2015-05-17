#region Using Directives

using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Tattoo.Common.Strings;
using Tattoo.Service.Contracts;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Validation.AccountValidators
{
    public class RegisterModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterModelValidator(IUserService userService)
        {
            RuleFor(model => model.Email)
                .EmailAddress()
                .NotNull();
            RuleFor(model => model.Password)
                .NotNull()
                .Length(6, 100);
            RuleFor(model => model.ConfirmPassword)
                .Equal(x => x.Password);

            Custom(
                model =>
                    userService.GetUsers(model.Email).Any()
                        ? new ValidationFailure("Email", Resources.ValMsg_EmailExist)
                        : null);
        }
    }
}