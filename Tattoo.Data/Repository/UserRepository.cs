#region Using Directives

using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

#endregion

namespace Tattoo.Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
    }

    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}