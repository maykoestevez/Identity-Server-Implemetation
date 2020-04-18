using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityFromScratch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Adding logger 
            LoggerFactory.Create(builder =>
             {
                 builder
                 .AddFilter("Microsoft", LogLevel.Warning)
                 .AddFilter("System", LogLevel.Warning)
                 .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                 .AddConsole()
                 .AddEventLog();
             });

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

       
    }
}
