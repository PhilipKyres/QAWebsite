using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QAWebsite.Models.Enums;

namespace QAWebsite.Models.QuestionModels
{
    public abstract class Rating
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(450)]
        [ForeignKey("ApplicationUser")]
        public string RatedBy { get; set; }

        [Required]
        public Ratings RatingValue { get; set; }

        public abstract string FkId { get; set; }
    }
}