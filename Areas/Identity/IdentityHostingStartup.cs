using System;
using HomeFinder.Areas.Identity.Data;
using HomeFinder.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(HomeFinder.Areas.Identity.IdentityHostingStartup))]
namespace HomeFinder.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<HomeFinderContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("HomeFinderContextConnection")));

                services.AddDefaultIdentity<HomeFinderUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<HomeFinderContext>();
            });
        }
    }
}