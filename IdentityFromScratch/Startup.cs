using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityFromScratch
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding service for Identity Third step
            services.AddIdentityServer()
                     .AddDeveloperSigningCredential() // Provide a way to sign the token to make it unique
                    .AddInMemoryApiResources(InMemoryConfiguration.ApiResources())
                    .AddInMemoryClients(InMemoryConfiguration.GetClients())
                    .AddTestUsers(InMemoryConfiguration.TestUsers().ToList());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory logger)
        {
       
            app.UseDeveloperExceptionPage();

            //adding Identity server middleware :second step
            app.UseIdentityServer();


        }
    }
}
