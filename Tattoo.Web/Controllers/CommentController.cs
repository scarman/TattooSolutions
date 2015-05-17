using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Service.Contracts;
using Tattoo.Web.Core.Extensions;
using Tattoo.Web.Models;
using Constants = Tattoo.Common.Constants;

namespace Tattoo.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IElementService _elementService;
        private readonly ISelectionService _selectionService;

        public CommentController(ICommentService commentService, IUserService userService, IElementService elementService, ISelectionService selectionService)
        {
            _commentService = commentService;
            _userService = userService;
            _elementService = elementService;
            _selectionService = selectionService;
        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        //AJAX: Comment/GetPaged
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPaged(int page = 1, int size = Constants.DefaultPageSize)
        {
            var comments =
                Mapper.Map<IPagedList<CommentViewModel>>(_commentService.GetCommentsPage(new PageInfo(page, size)));
            return PartialView("_CommentsList", comments);
        }

        //AJAX: Comment/GetPagedCreate
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult GetPagedCreate(string idElement, int page = 1, int size = Constants.DefaultPageSize)
        {
            var comments =
                Mapper.Map<IPagedList<CommentViewModel>>(_commentService.GetCommentsPageByCreate(new PageInfo(page, size), idElement));
            return PartialView("_CommentsListForCreate", comments);
        }

        // GET: Comment/Create
        public ActionResult Create(string id)
        {
            var author = _userService.GetUserByUserName(User.Identity.Name);
            var model = new CommentFormModel
            {
                Author = Mapper.Map<UserInfoCommentsViewModel>(_userService.GetUserByUserName(User.Identity.Name)),
                AuthorName = author.UserName,
                DateCreated = DateTime.Now,
                Element = Mapper.Map<ElementViewModel>(_elementService.Find(id)),
                IdElement = id
            };
            model.Element.Like = _selectionService.FindSelectionByType(author.Id, id, TypeSelection.Like);
            model.Element.Original = _selectionService.FindSelectionByType(author.Id, id, TypeSelection.Original);
            model.Element.Follows = _selectionService.FindSelectionByType(author.Id, id, TypeSelection.Follow);

            //Sacando la ruta de donde viene
            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                ViewBag.Url = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();

            return View(model);
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentFormModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var comment = Mapper.Map<Comment>(model);
                comment.Author = _userService.GetUserByUserName(model.AuthorName);
                comment.Element = _elementService.Find(model.IdElement);
                comment.DateCreated = DateTime.Now;

                var results = _commentService.Create(comment).ToList();

                if (!results.Any())
                    return RedirectToAction("Create", new { id = comment.Element.Id });

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
                _commentService.Delete(id);
                Logger.Warn(string.Format("Element Deleted: Id -> {0}, User who deleted -> {1}", id, User.Identity.Name));
                return Json(new SimpleResponse());
            }
            catch (Exception)
            {
                return Json(new SimpleResponse { Success = false });
            }
        }
    }
}