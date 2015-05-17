#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Controllers
{
    public class NoticeController : I18NController
    {
        private readonly INoticeService _noticeService;

        public NoticeController(INoticeService noticeService)
        {
            _noticeService = noticeService;
        }

        public ActionResult Index()
        {
            var notices = Mapper.Map<IEnumerable<NoticeViewModel>>(_noticeService.GetAll());
            return View(notices);
        }

        // AJAX: Notice/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var notices =
                Mapper.Map<IPagedList<NoticeViewModel>>(_noticeService.GetNoticesPage(new PageInfo(page, size)));
            return PartialView("_NoticeList", notices);
        }

        // GET: Notice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoticeFormModel model)
        {
            if (ModelState.IsValid)
            {
                var notice = Mapper.Map<Notice>(model);
                notice.Status = NoticeStatus.Open;

                var results = _noticeService.Create(notice).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // GET: Notice/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var notice = _noticeService.Find(id);
                var model = Mapper.Map<NoticeFormModel>(notice);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Notice/Edit
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoticeFormModel model)
        {
            if (ModelState.IsValid)
            {
                var notice = Mapper.Map<Notice>(model);
                var results = _noticeService.Update(notice).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // AJAX: Notice/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            try
            {
                _noticeService.Delete(id);
                Logger.Warn(string.Format("Notice Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }
        }

        // GET: Notice/List
        public ActionResult List()
        {
            var notice = Mapper.Map<IEnumerable<NoticeViewModel>>(_noticeService.GetAll());
            return View(notice);
        }

    }
}