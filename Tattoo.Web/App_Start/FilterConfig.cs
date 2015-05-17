#region Using Directives

using System.Web.Mvc;
using Tattoo.Web.Core.ActionFilters;

#endregion

namespace Tattoo.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DebugLoggingAttribute());
#if !DEBUG
            filters.Add(new AuthorizeAttribute());
#endif
        }
    }
}