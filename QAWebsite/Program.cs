using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QAWebsite.Data;
using QAWebsite.Models;

namespace QAWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var ApplicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var services = scope.ServiceProvider;
               (DBInitializer.SeedAsync(ApplicationDbContext,
                services.GetRequiredService<UserManager<ApplicationUser>>(),
                services.GetRequiredService<RoleManager<ApplicationRole>>())).Wait();
            }
            
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
