using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QAWebsite.Models.QuestionViewModels
{
    public class EditViewModel
    {
        public EditViewModel() {}

        public EditViewModel(Question question, TaggingViewModel tags)
        {
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.Tags = tags.Tags;
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Tags { get; set; }
    }
}
