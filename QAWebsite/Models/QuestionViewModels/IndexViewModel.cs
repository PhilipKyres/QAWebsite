﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.QuestionViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(Question question, int rating, int answerCount, int flagCount)
        {
            this.AuthorId = question.AuthorId;
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content.Length < 35 ? question.Content : question.Content.Substring(0, 35) + "...";
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
            this.Rating = rating;
            this.QuestionTags = question.QuestionTags;
            this.AnswerCount = answerCount;
            this.FlagCount = flagCount;

            if (question.Author != null)
                this.AuthorName = question.Author.UserName;
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
        [Display(Name = "Author")]
        [ReadOnly(true)]
        public string AuthorName { get; set; }

        public int AnswerCount { get; set; }

        [Display(Name = "Flags")]
        public int FlagCount { get; set; }

        [Required]
        public int Rating { get; set; }

        [Display(Name = "Tags")]
        public ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
