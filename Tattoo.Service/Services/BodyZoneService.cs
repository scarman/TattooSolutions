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
    public class BodyZoneService : IBodyZoneService
    {
        private readonly IBodyZoneRepository _bodyZoneRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BodyZoneService(IBodyZoneRepository bodyZoneRepository, IUnitOfWork unitOfWork)
        {
            _bodyZoneRepository = bodyZoneRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExecResult> Create(BodyZone entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _bodyZoneRepository.Add(entity);
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

        public IEnumerable<ExecResult> Update(BodyZone entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _bodyZoneRepository.Update(entity);
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
                var bodyZone = _bodyZoneRepository.GetById(Guid.Parse(id));
                _bodyZoneRepository.Delete(bodyZone);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public BodyZone Find(string id)
        {
            return _bodyZoneRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<BodyZone> GetAll()
        {
            return _bodyZoneRepository.GetAll();
        }

        public IPagedList<BodyZone> GetBodyZonePage(PageInfo pageInfo)
        {
            return _bodyZoneRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                bodyZone => !string.IsNullOrEmpty(bodyZone.Id.ToString()), bodyZone => bodyZone.Zone);
        }

    }
}
