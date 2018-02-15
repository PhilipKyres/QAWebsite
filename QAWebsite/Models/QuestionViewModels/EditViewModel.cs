using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
