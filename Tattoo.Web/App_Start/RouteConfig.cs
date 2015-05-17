using System.Web.Mvc;
using System.Web.Routing;

namespace Tattoo.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Tattoo.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Elmah",
                url: "Errors"
            );

            routes.MapRoute(
                name: "VerifyEmail",
                url: "Account/Verify/{id}"
            );
        }
    }
}
