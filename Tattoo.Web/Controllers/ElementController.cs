using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Data.Repository;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Core.I18N;
using Tattoo.Web.Models;
using Constants = Tattoo.Common.Constants;

namespace Tattoo.Web.Controllers
{
    public class ElementController : I18NController
    {
        private readonly IElementService _elementService;
        private readonly IBodyZoneService _bodyZoneService;
        private readonly IUserService _userService;

        public ElementController(IElementService elementService, IBodyZoneService bodyZoneService, IUserService userService)
        {
            _elementService = elementService;
            _bodyZoneService = bodyZoneService;
            _userService = userService;
        }


        // GET: Default
        public ActionResult Index()
        {
            var element = Mapper.Map<IEnumerable<ElementViewModel>>(_elementService.GetAll());
            return View(element);
        }
        // AJAX: BodyZone/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var element =
                   Mapper.Map<IPagedList<ElementViewModel>>(_elementService.GetElementsPage(new PageInfo(page, size)));

            //Si no es Administrador, filtrar y mostrar solo los elementos del usuario
            if (!User.IsInRole("Administrador"))
            {
                var nick = _userService.Find(User.Identity.GetUserId()).Nick;
                element =
                    Mapper.Map<IPagedList<ElementViewModel>>(_elementService.GetElementsPageByUser(new PageInfo(page, size), nick));
            }
            return PartialView("_ElementList", element);
        }

        // GET: Element/Create
        public ActionResult Create()
        {
            var element = Mapper.Map<IEnumerable<BodyZoneViewModels>>(_bodyZoneService.GetAll());
            ViewBag.BodyZone = element.Select(item => new { item.Zone, Value = item.Id.ToString() });

            return View();
        }

        // POST: Element/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ElementFormModel model)
        {
            if (ModelState.IsValid)
            {
                var destinationFolder = Server.MapPath("~/Content/Images/Elements/");
                var postedFile = model.Url;

                if (postedFile.ContentLength > 0)
                {
                    var extension = Path.GetExtension(postedFile.FileName);
                    var fileName = Guid.NewGuid() + extension;
                    var path = Path.Combine(destinationFolder, fileName);

                    postedFile.SaveAs(path);

                    var element = Mapper.Map<Element>(model);
                    element.Zone = _bodyZoneService.Find(model.ZoneId);
                    element.Url = Url.Content("~/Content/Images/Elements/" + fileName);

                    var author = _userService.GetUserByUserName(User.Identity.Name);
                    element.Author = author;
                    element.Author.Nick = author.Nick;
                    element.DateCreated = DateTime.Now;
                    element.CountVisits = 0;

                    var results = _elementService.Create(element).ToList();

                    if (!results.Any())
                        return RedirectToAction("Index", "Home");

                    ModelState.AddModelErrors(results);
                }
            }
            return View(model);
        }

        //GET: Element/Details
        public ActionResult Details(string id)
        {
            var temp = Mapper.Map<Element>(_elementService.Find(id));
            temp.CountVisits += 1;
            var result = _elementService.Update(temp);

            var model = Mapper.Map<ElementViewModel>(_elementService.Find(id));
            return View(model);
        }


        // GET: Element/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var elements = _elementService.Find(id);
                var model = Mapper.Map<ElementEditFormModel>(elements);
                model.Url = elements.Url;

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Element/Edit
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ElementEditFormModel model)
        {
            if (ModelState.IsValid)
            {
                var elements = _elementService.Find(model.Id.ToString());
                elements.Description = model.Description;

                var results = _elementService.Update(elements).ToList();

                if (!results.Any())
                    return RedirectToAction("Index");

                ModelState.AddModelErrors(results);
            }

            return View(model);
        }

        // AJAX: Element/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            try
            {
                _elementService.Delete(id);
                Logger.Warn(string.Format("Element Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }
        }

        //AJAX: Element/UpdateBySelection
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateElementBySelection(string id, TypeSelection type)
        {
            try
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);
                var element = _elementService.Find(id);

                var results = _elementService.UpdateElementBySelection(element, type, user).ToList();

                if (!results.Any())
                {
                    element = _elementService.Find(id);
                    var count = 0;
                    switch (type)
                    {
                        case TypeSelection.Like:
                            count = element.CountLike;
                            break;
                        case TypeSelection.Follow:
                            count = element.CountFollows;
                            break;
                        case TypeSelection.Original:
                            count = element.CountOriginal;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("type");
                    }
                    return Json(new { Success = true, Count = count });
                }
                return Json(new SimpleResponse { Success = false, Message = results.First().Message });
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }

        }
    }
}