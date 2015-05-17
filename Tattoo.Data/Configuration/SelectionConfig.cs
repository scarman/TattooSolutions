using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;

namespace Tattoo.Data.Configuration
{
    public class SelectionConfig : EntityTypeConfiguration<Selection>
    {
        public SelectionConfig()
        {
            Property(s => s.ElementId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
      
    }
}
