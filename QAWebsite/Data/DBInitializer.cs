using Microsoft.AspNetCore.Identity;
using QAWebsite.Models;
using QAWebsite.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var userRole = roleManager.Roles.FirstOrDefault(role => role.NormalizedName == Roles.User);
            if (userRole == null)
            {
                userRole = new ApplicationRole(Roles.User);
                await roleManager.CreateAsync(userRole);
            }

            var adminRole = roleManager.Roles.FirstOrDefault(role => role.NormalizedName == Roles.Administrator);
            if (adminRole == null)
            {
                adminRole = new ApplicationRole(Roles.Administrator);
                await roleManager.CreateAsync(adminRole);
            }

            var admin = userManager.Users.FirstOrDefault(user => user.NormalizedUserName == "Admin@QAWebsite.ca".Normalize());
            if (admin == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Admin@QAWebsite.ca",
                    Email = "Admin@QAWebsite.ca",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "!Qaz2wsx");
                await userManager.AddToRoleAsync(user, Roles.Administrator);
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
