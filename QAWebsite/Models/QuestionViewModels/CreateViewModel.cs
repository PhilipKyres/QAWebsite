using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.QuestionViewModels
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(300, MinimumLength = 15, ErrorMessage = "Must be between 15 and 300 characters")]
        [MaxLength(300, ErrorMessage = "Maximum 300 characters")]
        [MinLength(15, ErrorMessage = "Minumum 15 characters")]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Tags (Comma separated)")]
        public string Tags { get; set; }
    }
}
