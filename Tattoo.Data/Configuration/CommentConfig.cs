using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Data.Entities;

namespace Tattoo.Data.Configuration
{
    public class CommentConfig : EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            Property(x => x.Id).HasColumnName("CommentId");
        }
    }
}
