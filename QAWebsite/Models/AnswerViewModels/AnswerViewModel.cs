using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.AnswerViewModels
{
    public class AnswerViewModel
    {
        public AnswerViewModel(Answer answer, string authorName)
        {
            this.Id = answer.Id;
            this.Content = answer.Content;
            this.CreationDate = answer.CreationDate;
            this.EditDate = answer.EditDate;
            this.QuestionId = answer.QuestionId;
            this.AuthorId = answer.AuthorId;
            this.Author = authorName;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [MaxLength(8)]
        [Display(Name = "Question Id")]
        public string QuestionId { get; set; }

        [MaxLength(450)]
        [Display(Name = "Author Id")]
        public string AuthorId { get; set; }

        [MaxLength(450)]
        [Display(Name = "Author")]
        public string Author { get; set; }
    }
}
