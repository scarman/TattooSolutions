#region Using Directives

using System;
using System.Text;
using System.Web.Mvc;
using Tattoo.Common;

#endregion

namespace Tattoo.Web.Core.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var message = new StringBuilder();
            message.Append(string.Format("Served action {0} from {1} controller.",
                filterContext.ActionDescriptor.ActionName,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName));

            Logger.Info(message);
        }
    }
}