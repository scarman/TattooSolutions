#region Using Directives

using Mvc.Mailer;
using Tattoo.Common.Enumerations;
using Tattoo.Common.Strings;

#endregion

namespace Tattoo.Web.Mailers
{
    public sealed class AppMailer : MailerBase, IAppMailer
    {
        public AppMailer()
        {
            MasterName = "_Layout";
        }

        public MvcMailMessage VerifyEmail(string userEmail, string verifyUrl)
        {
            ViewBag.Url = verifyUrl;
            return Populate(x =>
            {
                //x.Subject = Resources.Msg_HealthMaxCareerCenter;
                x.ViewName = "VerifyEmail";
                x.To.Add(userEmail);
            });
        }

        public MvcMailMessage NewPassword(string email, string pass)
        {
            ViewBag.Email = email;
            ViewBag.Pass = pass;
            return Populate(x =>
            {
                //x.Subject = Resources.Msg_HealthMaxCareerCenter;
                x.ViewName = "NewPassword";
                x.To.Add(email);
            });
        }

        public MvcMailMessage NotifyStatusChange(string email, EmploymentStatus status, string position)
        {
            ViewBag.Status = status.ToString();
            ViewBag.Position = position;
            return Populate(x =>
            {
                //x.Subject = Resources.Msg_HealthMaxCareerCenter;
                x.ViewName = "StatusChange";
                x.To.Add(email);
            });
        }
    }
}