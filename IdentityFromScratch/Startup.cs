using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            // Adding service for Identity from in memory configuration
            //services.AddIdentityServer()
            //         .AddDeveloperSigningCredential() // Provide a way to sign the token to make it unique
            //        .AddInMemoryApiResources(InMemoryConfiguration.ApiResources())
            //        .AddInMemoryClients(InMemoryConfiguration.GetClients())
            //        .AddTestUsers(InMemoryConfiguration.TestUsers().ToList());

            //Persiting IDP configuration

            const string connectionString = @"Data Source=identityDb;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                }).AddDeveloperSigningCredential() // Provide a way to sign the token
                  .AddTestUsers(InMemoryConfiguration.TestUsers().ToList());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            InitializeDatabase(app);

            app.UseDeveloperExceptionPage();

            //adding Identity server middleware :second step
            app.UseIdentityServer();

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();

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
