﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Tattoo.Common.Data;
using Tattoo.Data.Entities;

namespace Tattoo.Service.Contracts
{
    public interface INoticeService : ICrudService<Notice>
    {
        IPagedList<Notice> GetNoticesPage(PageInfo pageInfo);
    }
}
