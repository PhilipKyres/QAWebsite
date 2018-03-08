using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.AccountViewModels
{
    public class ProfileViewModel
    {
        [Key]
        public string Id { get; set; }

        public virtual byte[] UserImage { get; set; }

        public virtual int Upvotes { get; set; }

        public virtual int Downvotes { get; set; }

        [Display(Name = "About Me")]
        public virtual string AboutMe { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        public List<Question> QuestionList { get; set; }

        //TODO Add recent activity questions set here and in the controller/view for the profile page.
    }
}
