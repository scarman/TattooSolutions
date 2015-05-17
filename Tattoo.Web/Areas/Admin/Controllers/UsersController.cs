#region Using Directives

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Areas.Admin.Models;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Mailers;
using Constants = Tattoo.Common.Constants;

#endregion

namespace Tattoo.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : I18NController
    {
        private readonly IUserService _userService;
        private IAppMailer _mailer = new AppMailer();
        private UserManager<ApplicationUser> _userManager;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
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

        // GET: Admin/Users
        public ActionResult Index()
        {
            var model = Mapper.Map<IEnumerable<UserViewModel>>(_userService.GetUsers());
            return View(model);
        }

        // AJAX: Admin/Users/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var users = Mapper.Map<IPagedList<UserViewModel>>(_userService.GetUsersPage(new PageInfo(page, size)));
            return PartialView("_UsersList", users);
        }

        // AJAX: Admin/Users/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            try
            {
                _userService.Delete(id);
                Logger.Warn(string.Format("User Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse {Success = false});
            }
        }

        // AJAX: Manage/ChangeStatus
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ChangeStatus(UserStatus status, string id)
        {
            try
            {
                var user = _userService.Find(id);
                user.Status = status;

                _userService.UpdateUser(user);
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse {Success = false});
            }
        }

        // AJAX: Manage/ChangeRole
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ChangeRole(Role role, string id)
        {
            try
            {
                var user = _userService.Find(id);
                user.Roles.Clear();
                _userService.UpdateUser(user);

                _userManager.AddToRole(id, role.ToString());

                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse {Success = false});
            }
        }


        /// <summary>
        ///     Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }
    }
}