using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class Question
    {
        [Key]
        [MaxLength(8)]
        public string Id { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [MaxLength(450)]
        [ForeignKey("ApplicationUser")]
        public string AuthorId { get; set; }

        [Required]
        [Display(Name = "Number of Votes")]
        public int Votes { get; set; }
    }
}
