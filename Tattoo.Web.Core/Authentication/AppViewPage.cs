using System.Security.Claims;
using System.Web.Mvc;

namespace Tattoo.Web.Core.Authentication
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUser CurrentUser
        {
            get { return new AppUser(User as ClaimsPrincipal); }
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}