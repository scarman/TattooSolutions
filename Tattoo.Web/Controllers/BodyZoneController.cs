using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Models;

namespace Tattoo.Web.Controllers
{
    public class BodyZoneController : I18NController
    {
        private readonly IBodyZoneService _bodyZoneService;

        public BodyZoneController(IBodyZoneService bodyZoneService)
        {
            _bodyZoneService = bodyZoneService;
        }

        // GET: BodyZone
        public ActionResult Index()
        {
            var bodyZone = Mapper.Map<IEnumerable<BodyZoneViewModels>>(_bodyZoneService.GetAll());
            return View(bodyZone);
        }
        // AJAX: BodyZone/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var bodyZone =
                Mapper.Map<IPagedList<BodyZoneViewModels>>(_bodyZoneService.GetBodyZonePage(new PageInfo(page, size)));
            return PartialView("_BodyZoneList", bodyZone);
        }

        // GET: BodyZone/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BodyZone/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BodyZoneFormModels model)
        {
            if (ModelState.IsValid)
            {
                var position = Mapper.Map<BodyZone>(model);
                var results = _bodyZoneService.Create(position).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // GET: BodyZone/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var bodyZone = _bodyZoneService.Find(id);
                var model = Mapper.Map<BodyZoneFormModels>(bodyZone);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: BodyZone/Edit
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BodyZoneFormModels model)
        {
            if (ModelState.IsValid)
            {
                var bodyZone = Mapper.Map<BodyZone>(model);
                var results = _bodyZoneService.Update(bodyZone).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // AJAX: BodyZone/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            try
            {
                _bodyZoneService.Delete(id);
                Logger.Warn(string.Format("Body Zone Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }
        }
    }
}