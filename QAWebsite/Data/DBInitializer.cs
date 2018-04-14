using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using QAWebsite.Models.Enums;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QAWebsite.Models.UserModels;

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
                    EmailConfirmed = true,
                    IsEnabled = true
                };

                await userManager.CreateAsync(user, adminSection["AdministratorPassword"]);
                await userManager.AddToRoleAsync(user, Roles.ADMINISTRATOR.ToString());
            }
            
            config.GetSection("Achievements").GetChildren().ToList().ForEach(achievement =>
            {
                if (context.Achievement.FirstOrDefault(a => a.Title == achievement["title"]) == null)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(Directory.GetCurrentDirectory() + achievement["AchievementImage"]);
                        byte[] imageData = new byte[fileInfo.Length];
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            fs.Read(imageData, 0, imageData.Length);
                        }

                        context.Achievement.Add(
                            new Achievement
                            {
                                Id = Guid.NewGuid().ToString(),
                                Title = achievement["title"],
                                Description = achievement["description"],
                                Comparator = (Comparators)Enum.Parse(typeof(Comparators), achievement["comparator"]),
                                Threshold = Int32.Parse(achievement["Threshold"]),
                                AchievementImage = imageData,
                                Type = (AchievementType)Enum.Parse(typeof(AchievementType), achievement["AchievementType"]),
                            });
                    }
                    catch (Exception exception)
                    { //parsing issue move on to next
                    }
                }
            });



            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
