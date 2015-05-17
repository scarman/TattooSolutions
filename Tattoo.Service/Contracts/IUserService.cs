using System.Collections.Generic;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Data.Entities;

namespace Tattoo.Service.Contracts
{
    public interface IUserService
    {
        ApplicationUser GetUser(string userId);

        IEnumerable<ApplicationUser> GetUsers();

        IEnumerable<ApplicationUser> GetUsers(string username);

        ApplicationUser GetUserProfile(string userid);

        ApplicationUser GetUserByUserName(string username);

        IEnumerable<ApplicationUser> GetUserByUserId(string userid);

        IEnumerable<ApplicationUser> SearchUser(string searchString);

        bool CanAddUser(string email);

        void UpdateUser(ApplicationUser user);

        void SaveUser();

        void EditUser(string id, string firstname, string lastname, string email);

        //void SaveImageUrl(string userId, string imageUrl);

        IPagedList<ApplicationUser> GetUsersPage(PageInfo pageInfo);

        void Delete(string id);

        ApplicationUser Find(string id);

        IEnumerable<ExecResult> Update(ApplicationUser entity);

    }
}