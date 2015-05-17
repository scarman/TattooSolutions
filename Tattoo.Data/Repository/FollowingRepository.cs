using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

namespace Tattoo.Data.Repository
{
    public interface IFollowRepository : IRepository<Selection>
    {
    }

    public class FollowingRepository : RepositoryBase<Selection>, IFollowRepository
    {
        public FollowingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}