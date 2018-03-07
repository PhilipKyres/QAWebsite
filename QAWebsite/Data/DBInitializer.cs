using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QAWebsite.Models;
using QAWebsite.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Data
{
    public class DBInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context,UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            var userRole = roleManager.Roles.Where(role => role.NormalizedName == Roles.User).FirstOrDefault();
            if (userRole == null)
            {
                userRole = new ApplicationRole(Roles.User);
                await roleManager.CreateAsync(userRole);
            }
            var adminRole = roleManager.Roles.Where(role => role.NormalizedName == Roles.Administrator).FirstOrDefault();
            if (adminRole == null)
            {
                adminRole = new ApplicationRole(Roles.Administrator);
                await roleManager.CreateAsync(adminRole);
            }
            var admin = userManager.Users.Where(user => user.NormalizedEmail == "Admin@QAWebsite.ca".Normalize()).FirstOrDefault();
            if (admin == null)
            {
                var user = new ApplicationUser { UserName = "Admin@QAWebsite.ca", Email = "Admin@QAWebsite.ca" };
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user, "!Qaz2wsx");
                await userManager.AddToRoleAsync(user, Roles.Administrator);
            }

            context.SaveChanges();
        }
    }
}
