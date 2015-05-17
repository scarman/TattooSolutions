using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

namespace Tattoo.Data.Repository
{
    public interface ICommentRepository : IRepository<Comment>
    {
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
