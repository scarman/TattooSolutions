using System;
using System.Collections.Generic;
using System.Linq;
using Tattoo.Common;
using Tattoo.Common.Enumerations;
using Tattoo.Common.Strings;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;
using Tattoo.Data.Repository;
using Tattoo.Service.Contracts;

namespace Tattoo.Service.Services
{
    public class SelectionService : ISelectionService
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SelectionService(ISelectionRepository selectionRepository, IUnitOfWork unitOfWork)
        {
            _selectionRepository = selectionRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExecResult> Create(Selection entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _selectionRepository.Add(entity);
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

        public IEnumerable<ExecResult> Update(Selection entity)
        {
            var results = new List<ExecResult>();
            try
            {
                _selectionRepository.Update(entity);
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
                var selection = _selectionRepository.GetById(Guid.Parse(id));
                _selectionRepository.Delete(selection);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public Selection Find(string id)
        {
            return _selectionRepository.GetById(Guid.Parse(id));
        }

        public IEnumerable<Selection> GetAll()
        {
            return _selectionRepository.GetAll();
        }

        public bool FindSelectionByType(string idUser, string idElement, TypeSelection type)
        {
            var vote = _selectionRepository.GetAll()
                .Where(obj => obj.UserId == idUser && obj.ElementId.ToString() == idElement && obj.SelectionType == type);
            return vote.Any();
        }
    }
}
