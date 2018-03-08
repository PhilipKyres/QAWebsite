using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models.QuestionModels
{
    public class Flag
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Reason")]
        public int Reason { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [ForeignKey("Question")]
        [MaxLength(8)]
        [Required]
        public string QuestionId { get; set; }
    }
}
