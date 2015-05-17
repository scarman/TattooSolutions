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
    public class NoticeService : INoticeService
    {
        private readonly INoticeRepository _noticeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NoticeService(INoticeRepository noticeRepository, IUnitOfWork unitOfWork)
        {
            _noticeRepository = noticeRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExecResult> Create(Notice entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _noticeRepository.Add(entity);
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

        public IEnumerable<ExecResult> Update(Notice entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _noticeRepository.Update(entity);
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
                var notice = _noticeRepository.GetById(Guid.Parse(id));
                _noticeRepository.Delete(notice);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public Notice Find(string id)
        {
            return _noticeRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<Notice> GetAll()
        {
            return _noticeRepository.GetAll();
        }

        public IPagedList<Notice> GetNoticesPage(PageInfo pageInfo)
        {
            return _noticeRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                                 notice => !string.IsNullOrEmpty(notice.Id.ToString()), notice => notice.Id);
        }

        //public IPagedList<Notice> GetNoticesPageByCreate(PageInfo pageInfo, string idElement)
        //{
        //    var guid = Guid.Parse(idElement);
        //    return _noticeRepository.GetPageOrderInverse(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
        //                                     Notice => !string.IsNullOrEmpty(Notice.Id.ToString()) && Notice.Element.Id == guid, Notice => Notice.DateCreated);
        //}
    }
}
