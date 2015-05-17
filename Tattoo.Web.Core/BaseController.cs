#region Using Directives

using System.Security.Claims;
using System.Web.Mvc;
using Tattoo.Web.Core.Authentication;

#endregion

namespace Tattoo.Web.Core
{
    public abstract class BaseController : Controller
    {
        public AppUser CurrentUser
        {
            get { return new AppUser(User as ClaimsPrincipal); }
        }
    }
}