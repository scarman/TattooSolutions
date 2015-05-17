#region Using Directives

using FluentValidation;
using FluentValidation.Results;
using Tattoo.Common.Strings;
using Tattoo.Service.Contracts;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Validation.ManagementValidators
{
    public class PositionValidator : AbstractValidator<PositionFormModel>
    {
        public PositionValidator(IPositionService positionService)
        {
            RuleFor(model => model.Name).NotNull();

            Custom(
                model =>
                    positionService.Exists(model.Name)
                        ? new ValidationFailure("Email", Resources.ValMsg_PositionExist)
                        : null);
        }
    }
}