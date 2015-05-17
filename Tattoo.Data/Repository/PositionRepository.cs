#region Using Directives

using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

#endregion

namespace Tattoo.Data.Repository
{
    public interface IPositionRepository : IRepository<Position>
    {
    }

    public class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        public PositionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}