using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

namespace Tattoo.Data.Repository
{
    public interface IBodyZoneRepository : IRepository<BodyZone>
    {
    }

    public class BodyZoneRepository : RepositoryBase<BodyZone>, IBodyZoneRepository
    {
        public BodyZoneRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}