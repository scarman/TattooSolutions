#region Using Directives

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Reflection;
using Microsoft.AspNet.Identity.EntityFramework;
using Tattoo.Data.Entities;

#endregion

namespace Tattoo.Data
{
    public class TattooEntities : IdentityDbContext<ApplicationUser>
    {
        public TattooEntities()
            : base("TattooEntities")
        {
        }

        public DbSet<Selection> Follows { get; set; }

        public DbSet<BodyZone> BodyZones { get; set; }

        public DbSet<Element> Elements { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Notice> Notices { get; set; }

        public DbSet<Position> Positions { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add/Remove conventions here
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(10, 2));
            modelBuilder.Properties<DateTime>().Configure(config => config.HasColumnType("datetime2"));
            modelBuilder.Properties<Guid>()
                .Where(prop => prop.Name.Contains("Id"))
                .Configure(config => config.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));

            // Add configurations here
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}