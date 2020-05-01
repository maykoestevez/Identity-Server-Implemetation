using System;
using IdentityFromScratch.Areas.Identity.Pages.Account;
using IdentityFromScratch.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityFromScratch.Areas.Identity.IdentityHostingStartup))]
namespace IdentityFromScratch.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                const string connectionString = @"Data Source=identity;";
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

                // Identity Configuration
                services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                services.AddTransient<IEmailSender, EmailSender>();

            });
        }
    }
}