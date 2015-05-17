using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;

namespace Tattoo.Data.Configuration
{
    public class ElementConfig : EntityTypeConfiguration<Element>
    {
        public ElementConfig()
        {
            Property(x => x.Id).HasColumnName("ElementId");
            Property(d => d.Description).IsRequired();
            Property(d => d.Url).IsRequired();
        }
    }
}
