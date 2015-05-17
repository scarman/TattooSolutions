#region Using Directives

using System.Collections.Generic;
using PagedList;
using Tattoo.Common.Data;
using Tattoo.Data.Entities;

#endregion

namespace Tattoo.Service.Contracts
{
    public interface IPositionService : ICrudService<Position>
    {
        IEnumerable<Position> GetAllOpen();
        
        bool Exists(string name);

        IPagedList<Position> GetPositionsPage(PageInfo pageInfo);
    }
}