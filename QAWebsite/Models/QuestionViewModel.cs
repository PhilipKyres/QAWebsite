using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models
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
        public int Upvotes { get; set; }

        [Required]
        public int Downvotes { get; set; }

        public QuestionViewModel (Question question)
        {
            this.AuthorId = question.AuthorId;
            this.QuestionId = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
        }
    }
}
