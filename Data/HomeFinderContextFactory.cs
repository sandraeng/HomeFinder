using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HomeFinder.Data
{
    public class HomeFinderContextFactory : IDesignTimeDbContextFactory<HomeFinderContext>
    {
        public HomeFinderContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HomeFinderContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("HomeFinderContextConnection"));
            return new HomeFinderContext(optionsBuilder.Options);
        }
    }
}
