using Microsoft.AspNetCore.Identity;
using System;

namespace QAWebsite.Models.UserModels
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            this.CreatedDate = DateTime.Now;
        }
        public ApplicationRole(string roleName) : base(roleName)
        { this.CreatedDate = DateTime.Now; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
