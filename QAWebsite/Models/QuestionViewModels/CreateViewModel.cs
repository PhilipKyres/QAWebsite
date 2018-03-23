using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QAWebsite.Models.QuestionViewModels
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Tags (Comma separated)")]
        public string Tags { get; set; }
    }
}
