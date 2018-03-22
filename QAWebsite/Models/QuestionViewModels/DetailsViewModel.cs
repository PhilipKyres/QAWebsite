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
        public DetailsViewModel() { }

        public DetailsViewModel(Question question, string authorName, int rating, int flags, List<AnswerViewModel> answers, List<CommentViewModel> comments)
        {
            this.AuthorId = question.AuthorId;
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
            this.QuestionTags = question.QuestionTags;
            this.BestAnswerId = question.BestAnswerId;
            this.AuthorName = authorName;
            this.Rating = rating;
            this.Answers = answers;
            this.Comments = comments;
            this.Flags = flags;
        }

        [ReadOnly(true)]
        public string Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string AuthorId { get; set; }

        [Display(Name = "Author")]
        public string AuthorName { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Your Answer")]
        public string AnswerContent { get; set; }

        public string BestAnswerId { get; set; }

        [Display(Name = "Add Comment")]
        public string Comment { get; set; }

        public int Flags { get; set; }

        public ICollection<AnswerViewModel> Answers { get; set; }

        public ICollection<QuestionTag> QuestionTags { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
