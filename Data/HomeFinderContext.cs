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
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<HomeFinderImages> Images { get; set; }
        public DbSet<NoticeOfInterest> NoticeOfInterests { get; set; }
        public DbSet<PropertyFavoritedByUser> PropertyFavorited { get; set; }
        public DbSet<PropertyObject> PropertyObjects { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
    }
}
