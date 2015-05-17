#region Using Directives

using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Tattoo.Common.Strings;

#endregion

namespace Tattoo.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Field_UserName")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_OldPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ValMsg_PasswordTooShort", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_ConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ValMsg_PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(Resources), Name = "Field_Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Field_RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(ResourceType = typeof(Resources), Name = "Field_Nick")]
        public string Nick { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Field_Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Field_ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Name { get; set; }

        public string Nick { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string State { get; set; }
        
    } 
    
    public class UserInfoCommentsViewModel
    {
        public string Name { get; set; }

        public string Nick { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string AvatarUrl { get; set; }

        public string SizedPicture(int width)
        {
            return string.Format("{0}?h={1}", AvatarUrl, width);
        }

        public string SizedPicture(int height, int width)
        {
            return string.Format("{0}?h={1}&w={2}", AvatarUrl, width, height);
        }
        
    }

    public class PasswordRecoveryModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Field_Email")]
        public string Email { get; set; }
    }
    
    public class UpdateProfileFormModel
    {
        public HttpPostedFileBase Avatar { get; set; }

        public string AvatarUrl { get; set; }

        public string SizedPicture(int width)
        {
            return string.Format("{0}?h={1}", AvatarUrl, width);
        }

        public string SizedPicture(int height, int width)
        {
            return string.Format("{0}?h={1}&w={2}", AvatarUrl, width, height);
        }

        [Display(ResourceType = typeof(Resources), Name = "Field_Nick")]
        public string Nick { get; set; }


        [Display(Name = "Field_Country", ResourceType = typeof(Resources), Prompt = "Field_Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Field_State", Prompt = "Field_Country")]
        public string State { get; set; }

    }

}