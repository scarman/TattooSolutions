using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Tattoo.Common;
using Tattoo.Common.Data;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;

namespace Tattoo.Service.Contracts
{
    public interface IElementService : ICrudService<Element>
    {
        IPagedList<Element> GetElementsPage(PageInfo pageInfo);
        
        IPagedList<Element> GetElementsPageByUser(PageInfo pageInfo, string nick);

        IEnumerable<ExecResult> UpdateElementBySelection(Element entity, TypeSelection type, ApplicationUser user);
    }
}
