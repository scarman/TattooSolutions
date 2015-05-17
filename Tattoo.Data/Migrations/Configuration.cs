#region Using Directives

using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TattooEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TattooEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Roles.AddOrUpdate(e => e.Name, new IdentityRole(Role.Administrator.ToString()));
            context.Roles.AddOrUpdate(e => e.Name, new IdentityRole(Role.User.ToString()));
        }
    }
}