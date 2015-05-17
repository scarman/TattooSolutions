#region Using Directives

using System.Data.Entity.ModelConfiguration;
using Tattoo.Data.Entities;

#endregion

namespace Tattoo.Data.Configuration
{
    public class PositionConfig : EntityTypeConfiguration<Position>
    {
        /// <summary>
        ///     Initializes a new instance of EntityTypeConfiguration
        /// </summary>
        public PositionConfig()
        {
            Property(x => x.Id).HasColumnName("PositionId");

            //HasMany(iu => iu.Details).WithRequired(history => history.Position).WillCascadeOnDelete();
        }
    }
}