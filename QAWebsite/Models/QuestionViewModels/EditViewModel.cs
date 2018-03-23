using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.QuestionViewModels
{
    public class EditViewModel
    {
        public EditViewModel() {}

        public EditViewModel(Question question)
        {
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.Tags = string.Join(", ", question.QuestionTags.Select(x => x.Tag.Name));
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

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
