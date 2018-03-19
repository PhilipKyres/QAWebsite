using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.AccountViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public virtual byte[] UserImage { get; set; }

        [Display(Name = "Upvote")]
        public virtual int QuestionUpvotes { get; set; }

        [Display(Name = "Downvote")]
        public virtual int QuestionDownvotes { get; set; }

        [Display(Name = "Upvote")]
        public virtual int AnswerUpvotes { get; set; }

        [Display(Name = "Downvote")]
        public virtual int AnswerDownvotes { get; set; }

        [Display(Name = "About Me")]
        public virtual string AboutMe { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        public List<Question> QuestionList { get; set; }


        public List<Answer> AnswerList { get; set; }
        
    }
}
