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
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Controllers
{
    public class PositionController : I18NController
    {
        private readonly IPositionService _positionService;
        private readonly IElementService _elementService;

        public PositionController(IPositionService positionService, IElementService elementService)
        {
            _positionService = positionService;
            _elementService = elementService;
        }

        public ActionResult Index()
        {
            var positions = Mapper.Map<IEnumerable<PositionViewModel>>(_positionService.GetAll());
            return View(positions);
        }

        // AJAX: Position/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var positions =
                Mapper.Map<IPagedList<PositionViewModel>>(_positionService.GetPositionsPage(new PageInfo(page, size)));
            return PartialView("_PositionsList", positions);
        }

        // GET: Position/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Position/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionFormModel model)
        {
            if (ModelState.IsValid)
            {
                var position = Mapper.Map<Position>(model);
                var results = _positionService.Create(position).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // GET: Position/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var position = _positionService.Find(id);
                var model = Mapper.Map<PositionFormModel>(position);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Position/Edit
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionFormModel model)
        {
            if (ModelState.IsValid)
            {
                var position = Mapper.Map<Position>(model);
                var results = _positionService.Update(position).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // AJAX: Ingredient/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            try
            {
                _positionService.Delete(id);
                Logger.Warn(string.Format("Position Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }
        }

        // GET: Position/List
        public ActionResult List()
        {
            var elements = Mapper.Map<IEnumerable<ElementViewModel>>(_elementService.GetAll());
            return View(elements);
        }

        // GET: Position/Test
        public ActionResult Test()
        {
            return View();
        }

        // POST: Position/Test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Test(TestFormModel model)
        {
            if (ModelState.IsValid)
            {
                var results = new List<ExecResult>();
                //var results = _positionService.Update(position).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }
    }
}