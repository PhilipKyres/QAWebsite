using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace QAWebsite.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

    //    public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name = "About me")]
        public string AboutMe { get; set; }

        [Display(Name = "User Image")]
        public IFormFile UserImage{ get; set; }
    }
}
