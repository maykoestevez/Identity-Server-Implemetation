using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityFromScratch.Areas.Identity.Pages.Account;
using IdentityFromScratch.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityFromScratch
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"Data Source=identityServer;";

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddIdentityServer(opt =>
            {
                opt.UserInteraction.LoginUrl = "/Identity/Account/Login";
                opt.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
            }).AddConfigurationStore(options =>
               {
                   options.ConfigureDbContext =
                      builder => builder.UseSqlite(connectionString);

               }).AddDeveloperSigningCredential()
                 .AddAspNetIdentity<IdentityUser>();
         

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            InitializeDatabase(app);

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseIdentityServer();// Identity Server add authentication middleware
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapDefaultControllerRoute();
        });

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var clients = _configuration.GetSection("IdentityServer:Clients").Get<IEnumerable<Client>>();
                var apiResources = _configuration.GetSection("IdentityServer:ApiResources").Get<IEnumerable<ApiResource>>();
                var identityResources = _configuration.GetSection("IdentityServer:IdentityResources").Get<IEnumerable<IdentityResource>>();

                if (!context.Clients.Any())
                {
                    //get client from configuration
                    context.Clients.AddRange(clients);
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    context.IdentityResources.AddRange(identityResources);
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    context.ApiResources.AddRange(apiResources);
                    context.SaveChanges();
                }
            }
        }

    }
}
