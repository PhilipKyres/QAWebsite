using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class Answer
    {
        [Key]
        [MaxLength(36)]
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
        [ForeignKey("Question")]
        public string QuestionId { get; set; }

        [MaxLength(450)]
        [ForeignKey("ApplicationUser")]
        public string AuthorId { get; set; }
    }
}