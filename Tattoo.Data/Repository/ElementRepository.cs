using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

namespace Tattoo.Data.Repository
{
    public interface IElementRepository : IRepository<Element>
    {
    }

    public class ElementRepository : RepositoryBase<Element>, IElementRepository
    {
        public ElementRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}