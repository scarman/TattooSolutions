#region Using Directives

using System;
using Tattoo.Common.Enumerations;
using Tattoo.Web.Core.Enumerations;

#endregion

namespace Tattoo.Web.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToStyleString(this BootstrapStyle style)
        {
            switch (style)
            {
                case BootstrapStyle.Default:
                    return "default";
                case BootstrapStyle.Primary:
                    return "primary";
                case BootstrapStyle.Info:
                    return "info";
                case BootstrapStyle.Success:
                    return "success";
                case BootstrapStyle.Warning:
                    return "warning";
                case BootstrapStyle.Danger:
                    return "danger";
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
        }

        public static string Stringify(this EmploymentStatus status)
        {
            switch (status)
            {
                case EmploymentStatus.Applicant:
                    return "User";
                case EmploymentStatus.Preselected:
                    return "Pre Selected";
                case EmploymentStatus.Employee:
                    return "Employee";
                case EmploymentStatus.ExEmployee:
                    return "Ex Employee";
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }
    }
}