#region Using Directives

using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SoftPool.Cryptography.Enumerations;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Mailers;
using Tattoo.Web.Models;
using Tattoo.Common.Strings;

#endregion

namespace Tattoo.Web.Controllers
{
    [Authorize]
    //[ActionLogging]
    public class AccountController : I18NController
    {
        private readonly IUserService _userService;

        private IAppMailer _mailer = new AppMailer();
        private UserManager<ApplicationUser> _userManager;

        public AccountController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public IAppMailer Mailer
        {
            get { return _mailer; }
            set { _mailer = value; }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(model.Email, model.Password);

                if (user != null)
                {
                    switch (user.Status)
                    {
                        case UserStatus.Active:
                            await SignInAsync(user, model.RememberMe);
                            return RedirectToLocal(returnUrl);
                        case UserStatus.Inactive:
                            ModelState.AddModelError("", "User is pending for manual activation.");
                            break;
                        case UserStatus.Pending:
                            ModelState.AddModelError("", Resources.ValMsg_InactiveUser);
                            ViewBag.IsInactive = true;
                            ViewBag.Email = model.Email;
                            ViewBag.Url = Url.RouteUrl("VerifyEmail", new { id = user.Id }, "http");
                            break;
                        case UserStatus.Locked:
                            ModelState.AddModelError("", "User account is locked. Contact the administrator for more information.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return View(model);
                }

                ViewBag.IsInvalid = true;
                ModelState.AddModelError("", Resources.ValMsg_InvalidCredentials);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Nick = model.Nick,
                    Status = UserStatus.Pending,
                    EmploymentStatus = EmploymentStatus.Applicant,
                    Avatar = Url.Content("~/Content/Images/Avatars/Default/avatar_default.png")
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user.Id, Role.User.ToString());

                if (result.Succeeded && roleResult.Succeeded)
                {
                    try
                    {
                        Mailer.VerifyEmail(model.Email, Url.RouteUrl("VerifyEmail", new { id = user.Id }, "http")).Send();
                    }
                    catch (Exception)
                    {
                        ViewBag.Email = model.Email;
                        ViewBag.Url = Url.RouteUrl("VerifyEmail", new { id = user.Id }, "http");
                        return View("AccountVerification");
                    }
                    return RedirectToAction("Registered", "Account");
                }
                AddErrors(result);
                AddErrors(roleResult);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/Registered
        [AllowAnonymous]
        public ActionResult Registered()
        {
            return View();
        }

        // POST: /Account/Verify
        [AllowAnonymous]
        public async Task<ActionResult> Verify(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Status = UserStatus.Active;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ErrorVerification", "Home");
        }

        // POST: /Account/ResendVerificationEmail
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResendVerificationEmail(string email, string url)
        {
            try
            {
                Mailer.VerifyEmail(email, url).Send();
            }
            catch (Exception)
            {
                ViewBag.Email = email;
                ViewBag.Url = url;
                return View("AccountVerification");
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/PasswordRecovery
        [AllowAnonymous]
        public ActionResult PasswordRecovery()
        {
            return View();
        }

        // POST: /Account/PasswordRecovery
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PasswordRecovery(PasswordRecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new IdentityResult();
                var user = _userService.GetUserByUserName(model.Email);
                if (user != null)
                {
                    result = await _userManager.RemovePasswordAsync(user.Id);
                    if (result.Succeeded)
                    {
                        var newPass = SoftPool.Cryptography.CryptoManager.GenerateStrongPassword(size: PwdSize.Bits0064);
                        result = await _userManager.AddPasswordAsync(user.Id, newPass);
                        if (result.Succeeded)
                        {
                            Mailer.NewPassword(model.Email, newPass).Send();
                            return RedirectToAction("PasswordChanged");
                        }
                    }
                }
                else
                    ModelState.AddModelError("", "Incorrect email address or invalid user profile.");

                AddErrors(result);
            }
            return View(model);
        }

        [AllowAnonymous]
        // GET: /Account/PasswordChanged
        public ActionResult PasswordChanged()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            var result =
                await
                    _userManager.RemoveLoginAsync(User.Identity.GetUserId(),
                        new UserLoginInfo(loginProvider, providerKey));

            ManageMessageId? message = result.Succeeded
                ? ManageMessageId.RemoveLoginSuccess
                : ManageMessageId.Error;

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "Your password has been changed."
                    : message == ManageMessageId.SetPasswordSuccess
                        ? "Your password has been set."
                        : message == ManageMessageId.RemoveLoginSuccess
                            ? "The external login was removed."
                            : message == ManageMessageId.Error
                                ? "An error has occurred."
                                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    var result =
                        await
                            _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                                model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    AddErrors(result);
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result =
                        await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await _userManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, false);
                return RedirectToLocal(returnUrl);
            }
            // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View("ExternalLoginConfirmation",
                new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        // GET: /Account/Users
        [AllowAnonymous]
        public ActionResult Users()
        {
            var users = _userService.GetUsers();
            return View(users.Select(Mapper.Map<UserInfoViewModel>));
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = _userManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            identity.AddClaim(new Claim(ClaimTypes.Surname, user.Nick));

            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        public ActionResult UpdateProfile()
        {
            try
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);
                var model = Mapper.Map<UpdateProfileFormModel>(user);
                model.AvatarUrl = user.Avatar;

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UpdateProfileFormModel model)
        {
            if (ModelState.IsValid)
            {
                var destinationFolder = Server.MapPath("~/Content/Images/Avatars/");
                var postedFile = model.Avatar;
                var userAvatar = _userManager.FindById(User.Identity.GetUserId());

                if (postedFile != null)
                    if (postedFile.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(postedFile.FileName);
                        var fileName = Guid.NewGuid() + extension;
                        var path = Path.Combine(destinationFolder, fileName);

                        postedFile.SaveAs(path);

                        userAvatar.Avatar = Url.Content("~/Content/Images/Avatars/" + fileName);
                    }
                userAvatar.Country = model.Country;
                userAvatar.State = model.State;

                var results = _userManager.Update(userAvatar);

                if (results.Succeeded)
                    return RedirectToAction("Index");

            }
            return View(model);
        }

    }
}