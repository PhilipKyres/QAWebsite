using System;
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
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Tags (Comma separated)")]
        public string Tags { get; set; }
    }
}
