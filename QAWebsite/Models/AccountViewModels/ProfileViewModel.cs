using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.AccountViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string UserImage { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        public List<Question> QuestionList { get; set; }


        public List<Answer> AnswerList { get; set; }
        
    }
}
