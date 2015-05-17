#region Using Directives

using System.Data.Entity.ModelConfiguration;
using Tattoo.Data.Entities;

#endregion

namespace Tattoo.Data.Configuration
{
    public class ApplicationUserConfig : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfig()
        {
            Property(user => user.Nick).IsRequired();

            HasMany(c => c.Follows).WithOptional(application => application.User).WillCascadeOnDelete();
        }
    }
}