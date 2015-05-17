#region Using Directives

using System;
using System.ComponentModel.DataAnnotations;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Role Role { get; set; }

        public UserStatus Status { get; set; }
    }
}