using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Tattoo.Common;
using Tattoo.Common.Enumerations;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Models;
using Constants = Tattoo.Common.Constants;

namespace Tattoo.Web.Controllers
{
    [Authorize]
    public class HomeController : I18NController
    {
        private readonly IElementService _elementService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;
        private readonly ISelectionService _selectionService;

        public HomeController(IElementService elementService, ICommentService commentService, IUserService userService, ISelectionService selectionService)
        {
            _elementService = elementService;
            _commentService = commentService;
            _userService = userService;
            _selectionService = selectionService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            //if (User.IsInRole(Role.User.ToString()))
            //    return RedirectToAction("Index", "Application");

            //if (User.IsInRole(Role.Administrator.ToString()) || User.IsInRole(Role.SystemWorker.ToString()))
            //    return RedirectToAction("Dashboard", "Manage");

            var elements = Mapper.Map<IEnumerable<ElementViewModel>>(_elementService.GetAll().OrderByDescending(o => o.DateCreated).Take(15));
            foreach (var item in elements)
            {
                var author = _userService.GetUserByUserName(User.Identity.Name);
                if (author != null)
                {
                    item.Like = _selectionService.FindSelectionByType(author.Id, item.Id.ToString(), TypeSelection.Like);
                    item.Original = _selectionService.FindSelectionByType(author.Id, item.Id.ToString(), TypeSelection.Original);
                    item.Follows = _selectionService.FindSelectionByType(author.Id, item.Id.ToString(), TypeSelection.Follow);
                }
            }
            return View(elements);
        }

        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            var cookie = Request.Cookies[Constants.CultureCookieName];
            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie(Constants.CultureCookieName) { Value = culture, Expires = DateTime.Now.AddYears(1) };
            }
            Response.Cookies.Add(cookie);

            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.AbsoluteUri))
                return Redirect(Request.UrlReferrer.AbsoluteUri);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: /Home/Error
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        // GET: /Home/Error404
        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }

        // GET: /Home/ErrorVerification
        [AllowAnonymous]
        public ActionResult ErrorVerification()
        {
            return View();
        }

    }
}