#region Using Directives

using System;
using System.Diagnostics;
using System.Text;
using System.Web.Mvc;
using Tattoo.Common;

#endregion

namespace Tattoo.Web.Core.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DebugLoggingAttribute : ActionFilterAttribute
    {
        private const string StopwatchKey = "DebugLoggingStopWatch";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Logger.IsDebugEnabled)
            {
                var loggingWatch = Stopwatch.StartNew();
                filterContext.HttpContext.Items.Add(StopwatchKey, loggingWatch);

                var message = new StringBuilder();
                message.Append(string.Format("Executing controller {0}, action {1}",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName));

                Logger.Debug(message);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Logger.IsDebugEnabled)
            {
                if (filterContext.HttpContext.Items[StopwatchKey] != null)
                {
                    var loggingWatch = (Stopwatch)filterContext.HttpContext.Items[StopwatchKey];
                    loggingWatch.Stop();

                    long timeSpent = loggingWatch.ElapsedMilliseconds;

                    var message = new StringBuilder();
                    message.Append(string.Format("Finished executing controller {0}, action {1} - time spent {2}",
                        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                        filterContext.ActionDescriptor.ActionName,
                        timeSpent));

                    Logger.Debug(message);
                    filterContext.HttpContext.Items.Remove(StopwatchKey);
                }
            }
        }
    }
}