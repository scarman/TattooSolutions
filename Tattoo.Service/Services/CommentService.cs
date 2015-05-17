using System;
using System.Collections.Generic;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Strings;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;
using Tattoo.Data.Repository;
using Tattoo.Service.Contracts;

namespace Tattoo.Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExecResult> Create(Comment entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _commentRepository.Add(entity);
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

        public IEnumerable<ExecResult> Update(Comment entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _commentRepository.Update(entity);
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
                var position = _commentRepository.GetById(Guid.Parse(id));
                _commentRepository.Delete(position);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public Comment Find(string id)
        {
            return _commentRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<Comment> GetAll()
        {
            return _commentRepository.GetAll();
        }

        public IPagedList<Comment> GetCommentsPage(PageInfo pageInfo)
        {
            return _commentRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                                 comment => !string.IsNullOrEmpty(comment.Id.ToString()), comment => comment.DateCreated);
        }

        public IPagedList<Comment> GetCommentsPageByCreate(PageInfo pageInfo, string idElement)
        {
            var guid = Guid.Parse(idElement);
            return _commentRepository.GetPageOrderInverse(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                                             comment => !string.IsNullOrEmpty(comment.Id.ToString()) && comment.Element.Id == guid, comment => comment.DateCreated);
        }
    }
}
