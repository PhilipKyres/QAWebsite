using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.UserModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public byte[] UserImage { get; set; }

        public string AboutMe { get; set; }

        public bool IsEnabled { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<ApplicationUserAchievements> UserAchievements { get; set; }
    }
}
