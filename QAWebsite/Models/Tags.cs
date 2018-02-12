using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QAWebsite.Models
{
    public class TaggingViewModel
    {
        [Key]
        [MaxLength(8)]
        public string Id { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [MaxLength(450)]
        [ForeignKey("Question")]
        public string QuestionId { get; set; }
    }
}
