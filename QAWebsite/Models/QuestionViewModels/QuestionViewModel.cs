using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace QAWebsite.Models.QuestionViewModels
{
    public class QuestionViewModel
    {
        [MaxLength(8)]
        [ReadOnly(true)]
        public string QuestionId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [Required]
        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string AuthorId { get; set; }

        [Required]
        [ReadOnly(true)]
        public string AuthorName { get; set; }

        [Required]
        public int Rating { get; set; }

        public QuestionViewModel (Question question, string authorName, int rating)
        {
            this.AuthorId = question.AuthorId;
            this.QuestionId = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
            this.AuthorName = authorName;
            this.Rating = rating;
        }
    }
}
