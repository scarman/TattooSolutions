#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Strings;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;
using Tattoo.Data.Repository;
using Tattoo.Service.Contracts;

#endregion

namespace Tattoo.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public ApplicationUser GetUserProfile(string userid)
        {
            var userprofile = _userRepository.Get(u => u.Id == userid);
            return userprofile;
        }

        public ApplicationUser GetUserByUserName(string username)
        {
            var userprofile = _userRepository.Get(u => u.UserName == username);
            return userprofile;
        }

        #region IUserService Members

        public ApplicationUser GetUser(string userId)
        {
            return _userRepository.Get(u => u.Id == userId);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            var users = _userRepository.GetAll();
            return users;
        }

        public IEnumerable<ApplicationUser> GetUsers(string username)
        {
            var users =
                _userRepository.GetMany(
                    u => u.UserName.Contains(username))
                    .OrderBy(u => u.UserName)
                    .ToList();

            return users;
        }

        public bool CanAddUser(string username)
        {
            var user = _userRepository.Get(u => u.UserName == username);
            return user != null;
        }

        public void EditUser(string id, string firstname, string lastname, string email)
        {
            var user = GetUser(id);
            UpdateUser(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _userRepository.Update(user);
            SaveUser();
        }

        public IEnumerable<ApplicationUser> SearchUser(string searchString)
        {
            var users =
                _userRepository.GetMany(
                    u =>
                        u.UserName.Contains(searchString)).OrderBy(u => u.UserName);
            return users;
        }

        public void SaveUser()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<ApplicationUser> GetUserByUserId(string userid)
        {
            var users = new List<ApplicationUser>();
            foreach (var item in userid)
            {
                var user = _userRepository.GetById(item);
                users.Add(user);
            }
            return users;
        }

        #endregion

        public IPagedList<ApplicationUser> GetUsersPage(PageInfo pageInfo)
        {
            return _userRepository.GetPage(new PageInfo(pageInfo.PageNumber, pageInfo.PageSize),
                user => !string.IsNullOrEmpty(user.Id), user => user.UserName);
        }

        public void Delete(string id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                _userRepository.Delete(user);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format(Resources.LogMsg_ExceptionOn, GetType(), ex.Message));
                throw;
            }
        }

        public ApplicationUser Find(string id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<ExecResult> Update(ApplicationUser entity)
        {
            var results = new List<ExecResult>();
            try
            {
                var user = _userRepository.GetById(entity.Id);
                user.Avatar = entity.Avatar;
                _userRepository.Update(user);
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
    }
}