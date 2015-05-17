#region Using Directives

using System.Security.Claims;
using System.Security.Principal;

#endregion

namespace Tattoo.Web.Core.Authentication
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(IPrincipal principal)
            : base(principal)
        {
        }

        public string Nick
        {
            get { return FindFirst(ClaimTypes.Surname).Value; }
        }
    }
}