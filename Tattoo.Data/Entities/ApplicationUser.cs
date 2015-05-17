#region Using Directives

using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Nick { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public int? Ranking { get; set; }

        public string Avatar { get; set; }

        public UserStatus Status { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; }

        public virtual ICollection<Selection> Follows { get; set; }
    }
}