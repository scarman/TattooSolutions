using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;

namespace Tattoo.Service.Contracts
{
    public interface ISelectionService : ICrudService<Selection>
    {
        bool FindSelectionByType(string idUser, string idElement, TypeSelection type);
    }
}
