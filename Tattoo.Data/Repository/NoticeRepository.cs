#region Using Directives

using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

#endregion

namespace Tattoo.Data.Repository
{
    public interface INoticeRepository : IRepository<Notice>
    {
    }

    public class NoticeRepository : RepositoryBase<Notice>, INoticeRepository
    {
        public NoticeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}