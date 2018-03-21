using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QAWebsite.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual byte[] UserImage { get; set; }

        public virtual string AboutMe { get; set; }
        
        public virtual ICollection<ApplicationUserAchievements> UserAchievements { get; set; }
        public bool IsEnabled { get; set; }
    }
}
