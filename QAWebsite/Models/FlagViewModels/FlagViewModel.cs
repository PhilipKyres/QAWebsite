using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QAWebsite.Models.Enums;

namespace QAWebsite.Models.FlagViewModels
{
    public class FlagViewModel
    {
        public FlagViewModel() {}

        public FlagViewModel(Question question)
        {
            this.QuestionId = question.Id;
        }

        [ReadOnly(true)]
        [HiddenInput(DisplayValue = false)]
        public string QuestionId { get; set; }

        [Required]
        [Display(Name = "Reason")]
        [EnumDataType(typeof(FlagType))]
        public FlagType SelectedReason { get; set; }
       
        [Required]
        public string Content { get; set; }
    }
}
