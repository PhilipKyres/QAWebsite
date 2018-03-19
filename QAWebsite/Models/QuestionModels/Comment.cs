using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public abstract class Comment
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

        [MaxLength(450)]
        [ForeignKey("ApplicationUser")]
        public string AuthorId { get; set; }
     
    }
}
