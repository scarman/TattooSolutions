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
    public class NoticeConfig : EntityTypeConfiguration<Notice>
    {
        public NoticeConfig()
        {
            Property(x => x.Id).HasColumnName("IdNotice");
            Property(d => d.Text).IsRequired();
        }
    }
}
