#region Using Directives

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

#endregion

namespace Tattoo.Service.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PositionService(IPositionRepository positionRepository, IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _unitOfWork = unitOfWork;
        }

        public bool Exists(string name)
        {
            var upperName = name.ToUpper();
            return _positionRepository.GetMany(item => item.Name.ToUpper() == upperName).Any();
        }

        public IEnumerable<ExecResult> Create(Position entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _positionRepository.Add(entity);
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

        public IEnumerable<ExecResult> Update(Position entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _positionRepository.Update(entity);
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
                var position = _positionRepository.GetById(Guid.Parse(id));
                _positionRepository.Delete(position);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public Position Find(string id)
        {
            return _positionRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<Position> GetAll()
        {
            return _positionRepository.GetAll(); 
        }

        public IEnumerable<Position> GetAllOpen()
        {
            return _positionRepository.GetMany(item => item.Status == PositionStatus.Open);
        }

        public IPagedList<Position> GetPositionsPage(PageInfo pageInfo)
        {
            return _positionRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                position => !string.IsNullOrEmpty(position.Name), position => position.Name);
        }
    }
}