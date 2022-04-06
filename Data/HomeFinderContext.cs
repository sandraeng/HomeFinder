using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeFinder.Data
{
    public class HomeFinderContext : IdentityDbContext<HomeFinderUser>
    {
        public HomeFinderContext(DbContextOptions<HomeFinderContext> options)
            : base(options)
        {
        }

        public HomeFinderContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<NoticeOfInterest>()
                .HasKey(c => new { c.PropertyObjectId, c.UserId });
            builder.Entity<PropertyFavoritedByUser>()
                .HasKey(c => new { c.PropertyObjectId, c.UserId });

            // Set ONDELETE cascade restrict
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<HomeFinderImages> Images { get; set; }
        public DbSet<NoticeOfInterest> NoticeOfInterests { get; set; }
        public DbSet<PropertyFavoritedByUser> PropertyFavorited { get; set; }
        public DbSet<PropertyObject> PropertyObjects { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LeaseType> LeaseTypes { get; set; }


        //public async Task<List<PropertyObject>> GetProperties()
        //{
        //    List<PropertyObject> propertyObjects;
        //    return propertyObjects = await PropertyObjects.Include(p => p.Address).Include(p => p.Realtor).ToListAsync();
        //}
    }
}
