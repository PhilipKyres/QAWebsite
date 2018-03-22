using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QAWebsite.Models.Enums;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Models.QuestionViewModels
{
    public class FlagViewModel
    {
        public FlagViewModel() {}

        public FlagViewModel(string questionId)
        {
            this.QuestionId = questionId;
        }
        public FlagViewModel(Flag flag, string questionId, string author)
        {
            this.Content = flag.Content;
            this.Reason = Enum.GetName(typeof(FlagType), flag.Reason);
            this.CreationDate = flag.CreationDate;
            this.Id = flag.Id;
            this.AuthorId = flag.AuthorId;
            this.Author = author;
            this.QuestionId = questionId;
        }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string QuestionId { get; set; }

        [Required]
        [Display(Name = "Reason")]
        [EnumDataType(typeof(FlagType))]
        public FlagType SelectedReason { get; set; }
       
        [Required]
        public string Content { get; set; }

        [MaxLength(36)]
        public string Id { get; set; }

        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public string AuthorId { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }
    }
}
