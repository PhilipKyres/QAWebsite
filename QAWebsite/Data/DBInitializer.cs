using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using QAWebsite.Models;
using QAWebsite.Models.Enums;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
           var config = new ConfigurationBuilder()
	        .SetBasePath(Directory.GetCurrentDirectory())
	        .AddJsonFile("seedingData.json")
	        .Build();
            
            var userRole = roleManager.Roles.FirstOrDefault(role => role.NormalizedName == Roles.USER.ToString());

            if (userRole == null)
            {
                userRole = new ApplicationRole(Roles.USER.ToString());
                await roleManager.CreateAsync(userRole);
            }

            var adminRole = roleManager.Roles.FirstOrDefault(role => role.NormalizedName == Roles.ADMINISTRATOR.ToString());
            if (adminRole == null)
            {
                adminRole = new ApplicationRole(Roles.ADMINISTRATOR.ToString());
                await roleManager.CreateAsync(adminRole);
            }
            var adminSection = config.GetSection("AdministratorAccount");
            var admin = userManager.Users.FirstOrDefault(user => user.NormalizedUserName == adminSection["AdministratorUsername"].Normalize());
            if (admin == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminSection["AdministratorUsername"],
                    Email = adminSection["AdministratorEmail"],
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminSection["AdministratorPassword"]);
                await userManager.AddToRoleAsync(user, Roles.ADMINISTRATOR.ToString());
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
