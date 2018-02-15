using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAWebsite.Models
{
    public class QuestionRating
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string RatedBy { get; set; }

        [ForeignKey("Question")]
        public string QuestionId { get; set; }

        [Required]
        public int RatingValue { get; set; }
    }
}
