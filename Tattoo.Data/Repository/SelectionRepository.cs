﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;

namespace Tattoo.Data.Repository
{
    public interface ISelectionRepository : IRepository<Selection>
    {
    }

    public class SelectionRepository : RepositoryBase<Selection>, ISelectionRepository
    {
        public SelectionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}