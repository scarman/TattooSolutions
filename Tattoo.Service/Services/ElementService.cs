using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Common.Strings;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;
using Tattoo.Data.Repository;
using Tattoo.Service.Contracts;

namespace Tattoo.Service.Services
{
    public class ElementService : IElementService
    {
        private readonly IElementRepository _elementRepository;
        private readonly ISelectionRepository _selectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ElementService(IElementRepository elementRepository, ISelectionRepository selectionRepository, IUnitOfWork unitOfWork)
        {
            _elementRepository = elementRepository;
            _selectionRepository = selectionRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExecResult> Create(Element entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _elementRepository.Add(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));

                results.Add(new ExecResult
                {
                    MemberName = null,
                    Message = ex.Message,
                });
            }

            return results;
        }

        public IEnumerable<ExecResult> Update(Element entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _elementRepository.Update(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));

                results.Add(new ExecResult
                {
                    MemberName = null,
                    Message = ex.Message,
                });
            }

            return results;
        }

        public IEnumerable<ExecResult> UpdateElementBySelection(Element entity, TypeSelection type, ApplicationUser user)
        {
            var results = new List<ExecResult>();
            try
            {
                if (type == TypeSelection.Follow)
                    entity.CountFollows++;
                if (type == TypeSelection.Like)
                    entity.CountLike++;
                if (type == TypeSelection.Original)
                    entity.CountOriginal++;

                var obj = new Selection
                {
                    ElementId = entity.Id,
                    UserId = user.Id,
                    SelectionType = type
                };

                entity.Selection.Add(obj);

                _elementRepository.Update(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));

                results.Add(new ExecResult
                {
                    MemberName = null,
                    Message = ex.Message,
                });
            }
            return results;
        }

        public void Delete(string id)
        {
            try
            {
                var position = _elementRepository.GetById(Guid.Parse(id));
                _elementRepository.Delete(position);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public Element Find(string id)
        {
            return _elementRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<Element> GetAll()
        {
            return _elementRepository.GetAll();
        }

        public IPagedList<Element> GetElementsPage(PageInfo pageInfo)
        {
            return _elementRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                     element => !string.IsNullOrEmpty(element.Id.ToString()), element => element.DateCreated);
        }

        public IPagedList<Element> GetElementsPageByUser(PageInfo pageInfo, string nick)
        {
            return _elementRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                element => element.Author.Nick == nick, element => element.DateCreated);
        }
    }
}
