using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.QuestionViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel(Question question, string authorName, int rating, List<AnswerViewModel> answers)
        {
            this.AuthorId = question.AuthorId;
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
            this.AuthorName = authorName;
            this.Rating = rating;
            this.Answers = answers;
            this.QuestionTags = question.QuestionTags;
        }

        [MaxLength(8)]
        [ReadOnly(true)]
        public string Id { get; set; }

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

        [Required]
        [Display(Name = "Your Answer")]
        public string AnswerContent { get; set; }

        public string BestAnswerId { get; set; }

        public ICollection<AnswerViewModel> Answers { get; set; }

        public ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
