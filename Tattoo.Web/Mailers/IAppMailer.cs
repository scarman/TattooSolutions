#region Using Directives

using Mvc.Mailer;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Web.Mailers
{
    public interface IAppMailer
    {
        MvcMailMessage VerifyEmail(string userEmail, string verifyUrl);

        MvcMailMessage NewPassword(string email, string pass);

        MvcMailMessage NotifyStatusChange(string email, EmploymentStatus status, string position);
    }
}