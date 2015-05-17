#region Using Directives

using System.Collections.Generic;
using System.Web.Mvc;
using Tattoo.Common;

#endregion

namespace Tattoo.Web.Core.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddModelErrors(this Controller controller, IEnumerable<ExecResult> execResults,
            string defaultErrorKey = null)
        {
            if (execResults != null)
            {
                foreach (var result in execResults)
                {
                    if (!string.IsNullOrEmpty(result.MemberName))
                        controller.ModelState.AddModelError(result.MemberName, result.Message);
                    else if (defaultErrorKey != null)
                        controller.ModelState.AddModelError(defaultErrorKey, result.Message);
                    else
                        controller.ModelState.AddModelError(string.Empty, result.Message);
                }
            }
        }

        public static void AddModelErrors(this ModelStateDictionary modelState,
            IEnumerable<ExecResult> execResults, string defaultErrorKey = null)
        {
            if (execResults == null) return;

            foreach (var result in execResults)
            {
                if (!string.IsNullOrEmpty(result.MemberName))
                    modelState.AddModelError(result.MemberName, result.Message);
                else if (defaultErrorKey != null)
                    modelState.AddModelError(defaultErrorKey, result.Message);
                else
                    modelState.AddModelError(string.Empty, result.Message);
            }
        }
    }
}