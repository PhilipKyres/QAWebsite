using Microsoft.AspNetCore.Mvc;
using QAWebsite.Models.AnswerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QAWebsite.Models.QuestionViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel() { }

        public DetailsViewModel(Question question, List<AnswerViewModel> answers)
        {
            this.Id = question.Id;
            this.Title = question.Title;
            this.Content = question.Content;
            this.CreationDate = question.CreationDate;
            this.EditDate = question.EditDate;
            this.AuthorId = question.AuthorId;
            this.BestAnswer = question.BestAnswerId;

            this.Answers = answers;
            this.AnswerCount = answers.Count;

        }

        [Required]
        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [ReadOnly(true)]
        public string Title { get; set; }

        [ReadOnly(true)]
        public string Content { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string AuthorId { get; set; }

        [ReadOnly(true)]
        public List<AnswerViewModel> Answers { get; set; }

        [Required]
        [Display(Name = "Your Answer")]
        public string AnswerContent { get; set; }
       
        [ReadOnly(true)]
        public int AnswerCount { get;}
        
        public string BestAnswer { get; set; }

    }
}
