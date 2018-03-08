using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            this.CreatedDate = DateTime.Now;
        }
        public ApplicationRole(string roleName) : base(roleName)
        { this.CreatedDate = DateTime.Now; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
